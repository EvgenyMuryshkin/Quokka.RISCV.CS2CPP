# Quokka.RISCV.CS2CPP

C# to CPP translation library for Quokka RISC-V integration.

## Installation:

### Prerequisites
* FPGA Board, there are plenty around from big vendors and open source community.
* FPGA Dev Software. 
* Basic knowledge of what is FPGA, how to configure project, run through toolchain and configure your board.
* Plenty of time to fiddle with stuff

### Dev Environment
* [GIT](https://git-scm.com/downloads).
* [Microsoft Visual Studio](https://visualstudio.microsoft.com/downloads/). Community edition is free. Make sure to select C#, .NET Core and Web development options.
* [Docker](https://www.docker.com/get-started). 

## Setup
RISC-V toolchain should be run in local docker container. 

Clone [Quokka RISCV Docker Repository](https://github.com/EvgenyMuryshkin/Quokka.RISCV.Docker)

`git clone https://github.com/EvgenyMuryshkin/Quokka.RISCV.Docker.git`

Run docker image build from Quokka.RISCV.Docker folder

`docker build . -t qrv`

That will take couple of hours and about 20GB of disk space on peak. You should have image with name 'qrv' at the end.
Container exposes two ports:
* 10000 for webmin
* 15000 for API server

Spin a container from this image with port mappings

`docker run -it -p 10000:10000 -p 15000:15000 qrv /bin/bash ./launch`

This command will launch new container from qrv image, map container ports to local ports for both internal applications and run internal server.

Once container is up and running, test it from the browser

`http://localhost:15000/api/healthcheck/isalive`

It should reply with `true`. Now you have integration server up and running.

## CS2CPP
Clone repository with translator and test cases

`git clone https://github.com/EvgenyMuryshkin/Quokka.RISCV.CS2CPP.git`

Run Visual Studio and open CS2CPP solution.
On tests windows, there will be some test cases.
Special test is `CompileFromIntermediateTest()`. It runs current content from intermediate folder on RISCV toolchain, so you can fiddle with intermediate files.

### Structure of Intermediate folder
Intermediate data is stored under `Quokka.RISCV.CS2CPP.Tests\Intermediate`

There are 3 parts
* FirmwareSource - all source files that will be send to compilation with RISCV.
* FirmwareOutput - everything that was produced by compilation (bin, elf, map etc)
* HardwareSource - verilog output, where PicoRV32 is integrated with external registers and memory blocks on a bus.

There are [default integration templates](https://github.com/EvgenyMuryshkin/Quokka.RISCV.Docker.Server/tree/master/Quokka.RISCV.Integration/Resources). You can override them if required.

### Blinker

From Test window, run `DMA_Register`

It should run successfully if everything was properly configured.

Result of the test is `hardware.v` in `Intermediate\HardwareSource` folder.
This file is self contained, has application binary embedded into memory init section.

*NOTE! There is 512 bytes limit on applicatiion, so there is a good change that you would not be able to run anything except blinker on current version :) But you can change that in `TranslateSourceFiles` method by simply increasing size of `firmware` memory block*

Hardware output was built using default diagnostics template and has bunch of signal on top module.

### Hardware

Create new project with EDA tool for your board.
Copy `hardware.v` to this project.

In order to run blinker, please clean up module interface. Something like this will do

```verilog
module RVTest (
  input clk,
  input resetn,
  output LED1,
  output LED2,	
 output LED3
);
```

CPU blinks about every 25 clock cycles. If you run 50MHz board, you can get visible LED flash using bits from 18-24 bits range.

```verilog
assign LED1 = BlinkerSource_DMA_Counter[19];
assign LED2 = BlinkerSource_DMA_Counter[21];
assign LED3 = BlinkerSource_DMA_Counter[23];
```

Clean up all debug output assignments in order to compile you project.
Configure all pins for your board.
Run synthesis and program your board.

Once run and reset - you should see LED blinks!

Feel free to contribute, raise issues and suggestions.
Project is in early stages, so any feedback will help.

Please follow me on [Twitter](https://twitter.com/ITMayWorkDev) for updates. (DM friendly)

Thanks.
