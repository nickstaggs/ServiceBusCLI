# ServiceBusCLI
[![Build status](https://dev.azure.com/NStaggs/service-bus-cli/_apis/build/status/service-bus-cli-CI)](https://dev.azure.com/NStaggs/service-bus-cli/_build/latest?definitionId=3)
[![Build status](https://vsrm.dev.azure.com/NStaggs/_apis/public/Release/badge/f36463d8-bf69-4677-837c-30f631eeedb0/1/1)](https://dev.azure.com/NStaggs/service-bus-cli/_release?definitionId=1)

## CLI to send and receive messages from Azure Service Bus

## Usage: read

`sbcli read [-c] [-q] [-t] [-s] [-n] [-x] [-p] [-v]`
### options
    -c, connection string to service bus, required
    -q, queue name
    -t, topic name
    -s, subscription name
    -n, number of messages to read, default: 10
    -x, time to wait for messages in seconds, -1 for indefinite, default: 10
    -p, peek messages, i.e. don't consume them off the queue, default: false
    -v, verbose, see all logging messages, default: false
### examples
`sbcli read -c Endpoint=sb://servicebus.servicebus.windows.net/;SharedAccessKeyName=ReadWriteSharedAccessKey;SharedAccessKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -q testqueue -p -v`

`sbcli read -c Endpoint=sb://servicebus.servicebus.windows.net/;SharedAccessKeyName=ReadWriteSharedAccessKey;SharedAccessKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -t testtopic -s testsubscription -x -1`

## Usage: write

`sbcli write [-c] [-q] [-t] [-m] [-u] [-v]`
### options
    -c, connection string to service bus, required
    -q, queue name
    -t, topic name
    -m, message to send to service bus, required
    -u, user properties to be placed on message, format: <k1>:<v1>,<k2>:<v2>,...,<kn>:<vn> no spaces
    -v, verbose, see all logging messages, default: false
### examples
`sbcli write -c Endpoint=sb://servicebus.servicebus.windows.net/;SharedAccessKeyName=ReadWriteSharedAccessKey;SharedAccessKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -q testqueue -m "service bus message"`

`sbcli write -c Endpoint=sb://servicebus.servicebus.windows.net/;SharedAccessKeyName=ReadWriteSharedAccessKey;SharedAccessKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -t testtopic -m "service bus message" -u userProp1:12345,userProp2:12345`

## Setup

### Compile yourself

dotnet CLI is required if you would like to compile the project yourself: https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install

1. Clone project
   
   `git clone https://github.com/nickstaggs/ServiceBusCLI.git`
2. Change directory into project
   
   `cd ServiceBusCLI`
3. Run dotnet restore
   
   `dotnet restore`
4. Compile and package project/dependencies
   
   `dotnet publish -c Release -r [win-x64, linux-x64, osx-x64]`
5. Either add publish directory to path variable or navigate into publish directory to use
   
   if you don't add it to the path variable you will only be able to use sbcli in the publish directory

   `cd ServiceBusCLI/bin/Release/netcoreapp2.1/[win-x64, linux-x64, osx-x64]/publish/`

### Download
I have created a single executable for all major platforms using Azure Pipelines and can be downloaded using the links below

windows: https://www.dropbox.com/s/pzfdfskh45e76tn/sbcli.exe?dl=0

linux: https://www.dropbox.com/s/54241irx9q5jfx3/sbcli?dl=0

osx: https://www.dropbox.com/s/v0n50dc6cmghoga/sbcli?dl=0
