# .NETFlow

## Introduction
This library is a native C# implementation for Cisco's [NetFlow v5](https://www.cisco.com/c/en/us/td/docs/net_mgmt/netflow_collection_engine/3-6/user/guide/format.html), [NetFlow v9](https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html) and IANA's [IPFIX](https://tools.ietf.org/html/rfc7012) protocols, which has been written in context of the [SAPPAN Horizon 2020 project](https://sappan-project.eu). It provides an in-memory representation for the flow sets used in the protocol as well as a `NetFlowReader` and a `NetFlowWriter` or an `IpfixReader`and an `IpfixWriter`, respectively, to read and write binary netflows from streams.


## Building and testing
The Visual Studio solution should build right away in a Visual Studio 2019 or later installation with C# workload and support for .NET Core installed with all dependencies being installed from [Nuget](https://www.nuget.org). The tests are implemented using the C# testing framework for Visual Studio and can be run from the "Test" menu. Note that the tests, in constrast to the library itself, target .NET 5 as .NET Standard is not supported for test libraries.


## Getting started
The library is self contained and supported on .NET Standard 2.0 or later. You should be able to build everything out-of-the-box. The following example demonstrate the useage of the library.

### Usage

This following example demonstrates how to write a single NetFlow v9 packet containing a template and a single data record into a file.
```C#
using DotNetFlow.Netflow9;
using System;
using System.IO;
using System.Net;

var header = new PacketHeader(2, 0, 0);

var record = new TemplateRecord(256);
record.Fields.Add(new Field(FieldType.IPv4SourceAddress));
record.Fields.Add(new Field(FieldType.IPv4DestinationAddress));

var template = new TemplateFlowSet();
template.Templates.Add(record);

var data = new DataFlowSet(256);
data.Records.Add(IPAddress.Parse("192.168.1.12"));
data.Records.Add(IPAddress.Parse("10.5.12.254"));

using (var fs = File.OpenWrite("test.flow"))
using (var nw = new NetFlowWriter(fs)) {
    nw.Write(header);
    nw.Write(template);
    nw.Write(data);
}
```

The next example demonstrates how to restore the packet from the file. As the interpretation of a `DataFlowSet` usually requires users to correlate the data flows with the matching template flows, the library provides a `NetFlowView` that facilitates this work.

```C#
using DotNetFlow.Netflow9;
using System;
using System.IO;
using System.Net;

using (var fs = File.OpenRead("test.flow"))
using (var nr = new NetFlowReader(fs)) {
    var header = nr.ReadPacketHeader();
    var template = nr.ReadFlowSet() as TemplateFlowSet;
    var data = nr.ReadFlowSet() as DataFlowSet;

    var view = new NetFlowView(data, template);

    // Access by field index.
    {
        var src = view[0, 0];
        var dst = view[0, 1];
    }

    // Access by field type.
    {
        var src = view[0, FieldType.IPv4SourceAddress];
        var dst = view[0, FieldType.IPv4DestinationAddress];
    } 

    // Access by field.
    {
        var src = view[0, template.Fields[0]];
        var dst = view[0, template.Fields[1]];
    } 

    // Obtain a whole record. The dynamically created properties are named after
    // the type of the respective field.
    {
        var record = view[0];
        var src = record.IPv4SourceAddress;
        var src = record.IPv4DestinationAddress;
    }

    // Enumerate all records.
    foreach (dynamic record in view) {
        var src = record.IPv4SourceAddress;
        var src = record.IPv4DestinationAddress;
    }
}
```

As NetFlow and IPFIX data are in network byte order, all readers and writers perform the appropriate conversions. Therefore, copying data using the reader and the writer is an unnecessarily costly operation as it converts and interprets all of the data. In order to perform an efficient per-packet copy, the library provides extension methods for `Stream` that copy the data while only interpreting the strictly necessary information. The following sample copies a NetFlow v9 stream:

```C#
using DotNetFlow.Netflow9;
using System;
using System.IO;

using (var ss = File.OpenRead("source.flow"))
using (var ds = File.OpenWrite("destination.flow")) {
    try {
        while (true) {
            ss.CopyNetflow9Packet(ds);
        }
    } catch (EndOfStreamException) { }
}
```

## Acknowledgements
This project has received funding from the European Union’s Horizon 2020 research and innovation programme under grant agreement No. 833418.
