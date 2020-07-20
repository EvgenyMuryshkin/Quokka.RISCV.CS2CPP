`default_nettype none

module RVTest (
	input clk,
	input resetn,
	output         o_mem_valid,
	output         o_mem_instr,

	output  [31:0] 	o_mem_addr,
	output  [31:0] 	o_mem_wdata,
	output  [ 3:0] 	o_mem_wstrb,
	output 			o_dbg_mem_valid,
	output [31:0] 	o_dbg_mem_rdata,
	output [31:0] 	o_dbg_mem_wdata,
	output         	o_dbg_mem_we,
	output         	o_dbg_data_access,
	output         	o_dbg_mem_read,
	output         	o_dbg_mem_write,
	output [31:0]   o_dbg_tmp
);

	parameter [0:0] BARREL_SHIFTER = 1;
	parameter [0:0] ENABLE_MULDIV = 1;
	parameter [0:0] ENABLE_COMPRESSED = 1;
	parameter [0:0] ENABLE_COUNTERS = 1;
	parameter [0:0] ENABLE_IRQ_QREGS = 0;

	parameter integer MEM_WORDS = 256;
	parameter [31:0] STACKADDR = 32'h 0000_0800;// (2*MEM_WORDS);       // end of memory
	parameter [31:0] PROGADDR_RESET = 32'h 0000_0000;
	parameter [31:0] PROGADDR_IRQ = 32'h 0020_0000;
	
	// picorv signals
	wire 		cpu_mem_valid;
	wire 		cpu_mem_instr;
	wire 		cpu_mem_ready;
	wire [31:0] cpu_mem_addr;
	wire [31:0] cpu_mem_wdata;
	wire [3:0] 	cpu_mem_wstrb;
	wire [31:0] cpu_mem_rdata;
	
	wire		cpu_read_request;
	wire		cpu_write_request;
	assign cpu_read_request = cpu_mem_instr || cpu_mem_wstrb == 4'b0;
	assign cpu_write_request = cpu_mem_wstrb != 4'b0;

// BEGIN DATA_DECL
reg [31 : 0] firmware[0 : 511];
reg [31 : 0] DataDeclarationTestSource_SOC_Result;

// END DATA_DECL

// BEGIN DATA_CTRL
	// 32 bit memory logic for firmware
	wire 			firmware_ready;
	reg 			firmware_read_ready = 1'b0;
	reg 			firmware_write_ready = 1'b0;
	reg  [31:0] firmware_rdata = 32'b0;
	wire [31:0] firmware_wdata;	
	wire [31: 0]	firmware_address;
	wire 			firmware_we;
	reg  [1:0] 		firmware_write_state = 2'b0;
	wire 			firmware_address_valid;

	assign firmware_address_valid = cpu_mem_addr[31:20] == 12'h00;
	assign firmware_ready = firmware_read_ready || firmware_write_ready;
	
	assign firmware_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : firmware_rdata[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : firmware_rdata[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : firmware_rdata[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : firmware_rdata[7:0]
	};

	assign firmware_we = firmware_write_state == 2'b1 && firmware_address_valid;
	assign firmware_address = cpu_mem_addr[31:2];
	
	always @(posedge clk)
	begin
		firmware_read_ready <= 1'b0;
		firmware_write_ready <= 1'b0;
		firmware_write_state <= 2'b0;
		firmware_rdata <= 32'b0;

		if (resetn && cpu_mem_valid && firmware_address_valid)
			begin	
				firmware_read_ready <= cpu_read_request;
				firmware_write_ready <= cpu_write_request && firmware_write_state == 2'b1;
			
				if (cpu_write_request)
				begin
					case (firmware_write_state)
						0: begin
							firmware_write_state <= 2'b1;
							// read ready in next cycle
						end
						1: begin
							firmware_write_state <= 2'b0;
						end
					endcase
				end
			end

		if (firmware_we)
			begin
				firmware[firmware_address] <= firmware_wdata;
			end

		firmware_rdata <= firmware[firmware_address];
	end

	wire 		DataDeclarationTestSource_SOC_Result_ready;
	wire 		DataDeclarationTestSource_SOC_Result_we;
	wire [31:0] DataDeclarationTestSource_SOC_Result_wdata;	

	// byteenabled write
	assign DataDeclarationTestSource_SOC_Result_wdata = {
		cpu_mem_wstrb[3] ? cpu_mem_wdata[31:24] : DataDeclarationTestSource_SOC_Result[31:24],
		cpu_mem_wstrb[2] ? cpu_mem_wdata[23:16] : DataDeclarationTestSource_SOC_Result[23:16],
		cpu_mem_wstrb[1] ? cpu_mem_wdata[15:8]  : DataDeclarationTestSource_SOC_Result[15:8],
		cpu_mem_wstrb[0] ? cpu_mem_wdata[7:0]   : DataDeclarationTestSource_SOC_Result[7:0]
	};
	
	assign DataDeclarationTestSource_SOC_Result_ready = cpu_mem_addr[31:20] == 12'h800;
	assign DataDeclarationTestSource_SOC_Result_we = DataDeclarationTestSource_SOC_Result_ready && !cpu_mem_instr;
	
	// memory logic
	always @(posedge clk)
	begin
		if (resetn)
			begin
				if (DataDeclarationTestSource_SOC_Result_we)
					begin
						DataDeclarationTestSource_SOC_Result <= DataDeclarationTestSource_SOC_Result_wdata;
					end
			end
		else
			begin
				DataDeclarationTestSource_SOC_Result <= 32'b0;
			end
	end
	


// END DATA_CTRL

	// feedback to cpu
	assign cpu_mem_ready = firmware_ready || DataDeclarationTestSource_SOC_Result_ready;
	assign cpu_mem_rdata = firmware_ready ? firmware_rdata : DataDeclarationTestSource_SOC_Result_ready ? DataDeclarationTestSource_SOC_Result : 32'b0;
		
	assign o_mem_valid = 	cpu_mem_valid;
	assign o_mem_instr = 	cpu_mem_instr;
	assign o_mem_addr = 	cpu_mem_addr;
	assign o_mem_wdata = 	cpu_mem_wdata;
	assign o_mem_wstrb = 	cpu_mem_wstrb;
	
	picorv32 #(
		.STACKADDR(STACKADDR),
		.PROGADDR_RESET(PROGADDR_RESET),
		.PROGADDR_IRQ(PROGADDR_IRQ),
		.BARREL_SHIFTER(BARREL_SHIFTER),
		.COMPRESSED_ISA(ENABLE_COMPRESSED),
		.ENABLE_COUNTERS(ENABLE_COUNTERS),
		.ENABLE_MUL(ENABLE_MULDIV),
		.ENABLE_DIV(ENABLE_MULDIV),
		.ENABLE_IRQ(1),
		.ENABLE_IRQ_QREGS(ENABLE_IRQ_QREGS)
	) cpu (
		.clk         (clk        	),
		.resetn      (resetn     	),
		.mem_valid   (cpu_mem_valid ),
		.mem_instr   (cpu_mem_instr ),
		.mem_ready   (cpu_mem_ready ),
		.mem_addr    (cpu_mem_addr  ),
		.mem_wdata   (cpu_mem_wdata ),
		.mem_wstrb   (cpu_mem_wstrb ),
		.mem_rdata   (cpu_mem_rdata ),
		.irq         (0        	 	)
	);
	
	// connect debug and diagnostics
			
	assign o_dbg_mem_valid = firmware_ready;
	assign o_dbg_mem_rdata = firmware_rdata;
	assign o_dbg_mem_wdata = firmware_wdata;
	assign o_dbg_mem_we = firmware_we;

	//assign o_dbg_mem_rdata = data_rdata_part;
	//assign o_dbg_mem_wdata = data_read_address_part;
	//assign o_dbg_mem_we = data_we;
	
	//assign o_dbg_data_access = data_address_valid;
	assign o_dbg_mem_read = cpu_read_request;
	assign o_dbg_mem_write = cpu_write_request;
	
	initial
	begin
// BEGIN MEM_INIT
/*000*/firmware[0] = 32'h1740006F;
/*004*/firmware[1] = 32'h00000013;
/*008*/firmware[2] = 32'h00000013;
/*00C*/firmware[3] = 32'h00000013;
/*010*/firmware[4] = 32'h00102223;
/*014*/firmware[5] = 32'h00202423;
/*018*/firmware[6] = 32'h0000408B;
/*01C*/firmware[7] = 32'h00102623;
/*020*/firmware[8] = 32'h0000C08B;
/*024*/firmware[9] = 32'h00102823;
/*028*/firmware[10] = 32'h00001137;
/*02C*/firmware[11] = 32'hF8010113;
/*030*/firmware[12] = 32'h0000408B;
/*034*/firmware[13] = 32'h00112023;
/*038*/firmware[14] = 32'h00402083;
/*03C*/firmware[15] = 32'h00112223;
/*040*/firmware[16] = 32'h00802083;
/*044*/firmware[17] = 32'h00112423;
/*048*/firmware[18] = 32'h00312623;
/*04C*/firmware[19] = 32'h00412823;
/*050*/firmware[20] = 32'h00512A23;
/*054*/firmware[21] = 32'h00612C23;
/*058*/firmware[22] = 32'h00712E23;
/*05C*/firmware[23] = 32'h02812023;
/*060*/firmware[24] = 32'h02912223;
/*064*/firmware[25] = 32'h02A12423;
/*068*/firmware[26] = 32'h02B12623;
/*06C*/firmware[27] = 32'h02C12823;
/*070*/firmware[28] = 32'h02D12A23;
/*074*/firmware[29] = 32'h02E12C23;
/*078*/firmware[30] = 32'h02F12E23;
/*07C*/firmware[31] = 32'h05012023;
/*080*/firmware[32] = 32'h05112223;
/*084*/firmware[33] = 32'h05212423;
/*088*/firmware[34] = 32'h05312623;
/*08C*/firmware[35] = 32'h05412823;
/*090*/firmware[36] = 32'h05512A23;
/*094*/firmware[37] = 32'h05612C23;
/*098*/firmware[38] = 32'h05712E23;
/*09C*/firmware[39] = 32'h07812023;
/*0A0*/firmware[40] = 32'h07912223;
/*0A4*/firmware[41] = 32'h07A12423;
/*0A8*/firmware[42] = 32'h07B12623;
/*0AC*/firmware[43] = 32'h07C12823;
/*0B0*/firmware[44] = 32'h07D12A23;
/*0B4*/firmware[45] = 32'h07E12C23;
/*0B8*/firmware[46] = 32'h07F12E23;
/*0BC*/firmware[47] = 32'h0000C50B;
/*0C0*/firmware[48] = 32'h00010593;
/*0C4*/firmware[49] = 32'h00002083;
/*0C8*/firmware[50] = 32'h00100463;
/*0CC*/firmware[51] = 32'h000080E7;
/*0D0*/firmware[52] = 32'h00012083;
/*0D4*/firmware[53] = 32'h0200A00B;
/*0D8*/firmware[54] = 32'h00412083;
/*0DC*/firmware[55] = 32'h0200A08B;
/*0E0*/firmware[56] = 32'h00812083;
/*0E4*/firmware[57] = 32'h00102223;
/*0E8*/firmware[58] = 32'h00C12183;
/*0EC*/firmware[59] = 32'h01012203;
/*0F0*/firmware[60] = 32'h01412283;
/*0F4*/firmware[61] = 32'h01812303;
/*0F8*/firmware[62] = 32'h01C12383;
/*0FC*/firmware[63] = 32'h02012403;
/*100*/firmware[64] = 32'h02412483;
/*104*/firmware[65] = 32'h02812503;
/*108*/firmware[66] = 32'h02C12583;
/*10C*/firmware[67] = 32'h03012603;
/*110*/firmware[68] = 32'h03412683;
/*114*/firmware[69] = 32'h03812703;
/*118*/firmware[70] = 32'h03C12783;
/*11C*/firmware[71] = 32'h04012803;
/*120*/firmware[72] = 32'h04412883;
/*124*/firmware[73] = 32'h04812903;
/*128*/firmware[74] = 32'h04C12983;
/*12C*/firmware[75] = 32'h05012A03;
/*130*/firmware[76] = 32'h05412A83;
/*134*/firmware[77] = 32'h05812B03;
/*138*/firmware[78] = 32'h05C12B83;
/*13C*/firmware[79] = 32'h06012C03;
/*140*/firmware[80] = 32'h06412C83;
/*144*/firmware[81] = 32'h06812D03;
/*148*/firmware[82] = 32'h06C12D83;
/*14C*/firmware[83] = 32'h07012E03;
/*150*/firmware[84] = 32'h07412E83;
/*154*/firmware[85] = 32'h07812F03;
/*158*/firmware[86] = 32'h07C12F83;
/*15C*/firmware[87] = 32'h0000C08B;
/*160*/firmware[88] = 32'h00402103;
/*164*/firmware[89] = 32'h00000013;
/*168*/firmware[90] = 32'h00000013;
/*16C*/firmware[91] = 32'h00000013;
/*170*/firmware[92] = 32'h0400000B;
/*174*/firmware[93] = 32'h00000093;
/*178*/firmware[94] = 32'h00000193;
/*17C*/firmware[95] = 32'h00000213;
/*180*/firmware[96] = 32'h00000293;
/*184*/firmware[97] = 32'h00000313;
/*188*/firmware[98] = 32'h00000393;
/*18C*/firmware[99] = 32'h00000413;
/*190*/firmware[100] = 32'h00000493;
/*194*/firmware[101] = 32'h00000513;
/*198*/firmware[102] = 32'h00000593;
/*19C*/firmware[103] = 32'h00000613;
/*1A0*/firmware[104] = 32'h00000693;
/*1A4*/firmware[105] = 32'h00000713;
/*1A8*/firmware[106] = 32'h00000793;
/*1AC*/firmware[107] = 32'h00000813;
/*1B0*/firmware[108] = 32'h00000893;
/*1B4*/firmware[109] = 32'h00000913;
/*1B8*/firmware[110] = 32'h00000993;
/*1BC*/firmware[111] = 32'h00000A13;
/*1C0*/firmware[112] = 32'h00000A93;
/*1C4*/firmware[113] = 32'h00000B13;
/*1C8*/firmware[114] = 32'h00000B93;
/*1CC*/firmware[115] = 32'h00000C13;
/*1D0*/firmware[116] = 32'h00000C93;
/*1D4*/firmware[117] = 32'h00000D13;
/*1D8*/firmware[118] = 32'h00000D93;
/*1DC*/firmware[119] = 32'h00000E13;
/*1E0*/firmware[120] = 32'h00000E93;
/*1E4*/firmware[121] = 32'h00000F13;
/*1E8*/firmware[122] = 32'h00000F93;
/*1EC*/firmware[123] = 32'h018000EF;
/*1F0*/firmware[124] = 32'h0000006F;
/*1F4*/firmware[125] = 32'h00000000;
/*1F8*/firmware[126] = 32'h00000000;
/*1FC*/firmware[127] = 32'h0605650B;
/*200*/firmware[128] = 32'h00008067;
/*204*/firmware[129] = 32'hFE010113;
/*208*/firmware[130] = 32'h00112E23;
/*20C*/firmware[131] = 32'h00812C23;
/*210*/firmware[132] = 32'h02010413;
/*214*/firmware[133] = 32'h0FF00513;
/*218*/firmware[134] = 32'hFE5FF0EF;
/*21C*/firmware[135] = 32'h35800793;
/*220*/firmware[136] = 32'hFEF42623;
/*224*/firmware[137] = 32'hFEC42703;
/*228*/firmware[138] = 32'h35800793;
/*22C*/firmware[139] = 32'h00F77C63;
/*230*/firmware[140] = 32'hFEC42783;
/*234*/firmware[141] = 32'h00478713;
/*238*/firmware[142] = 32'hFEE42623;
/*23C*/firmware[143] = 32'h0007A023;
/*240*/firmware[144] = 32'hFE5FF06F;
/*244*/firmware[145] = 32'h018000EF;
/*248*/firmware[146] = 32'h00000013;
/*24C*/firmware[147] = 32'h01C12083;
/*250*/firmware[148] = 32'h01812403;
/*254*/firmware[149] = 32'h02010113;
/*258*/firmware[150] = 32'h00008067;
/*25C*/firmware[151] = 32'hFE010113;
/*260*/firmware[152] = 32'h00812E23;
/*264*/firmware[153] = 32'h02010413;
/*268*/firmware[154] = 32'h00A00793;
/*26C*/firmware[155] = 32'hFEF407A3;
/*270*/firmware[156] = 32'h01400793;
/*274*/firmware[157] = 32'hFEF40723;
/*278*/firmware[158] = 32'hFE042423;
/*27C*/firmware[159] = 32'h06400793;
/*280*/firmware[160] = 32'hFEF42223;
/*284*/firmware[161] = 32'h000027B7;
/*288*/firmware[162] = 32'h71078793;
/*28C*/firmware[163] = 32'hFEF42023;
/*290*/firmware[164] = 32'hFEF44703;
/*294*/firmware[165] = 32'hFEE44783;
/*298*/firmware[166] = 32'h00F70733;
/*29C*/firmware[167] = 32'hFE842683;
/*2A0*/firmware[168] = 32'hFE442783;
/*2A4*/firmware[169] = 32'h40F687B3;
/*2A8*/firmware[170] = 32'h02F707B3;
/*2AC*/firmware[171] = 32'h00078713;
/*2B0*/firmware[172] = 32'hFE042783;
/*2B4*/firmware[173] = 32'h00F70733;
/*2B8*/firmware[174] = 32'h800007B7;
/*2BC*/firmware[175] = 32'h00E7A023;
/*2C0*/firmware[176] = 32'h00000013;
/*2C4*/firmware[177] = 32'h01C12403;
/*2C8*/firmware[178] = 32'h02010113;
/*2CC*/firmware[179] = 32'h00008067;
/*2D0*/firmware[180] = 32'h00000010;
/*2D4*/firmware[181] = 32'h00000000;
/*2D8*/firmware[182] = 32'h00527A01;
/*2DC*/firmware[183] = 32'h01017C01;
/*2E0*/firmware[184] = 32'h00020D1B;
/*2E4*/firmware[185] = 32'h00000024;
/*2E8*/firmware[186] = 32'h00000018;
/*2EC*/firmware[187] = 32'hFFFFFF18;
/*2F0*/firmware[188] = 32'h00000058;
/*2F4*/firmware[189] = 32'h200E4400;
/*2F8*/firmware[190] = 32'h88018148;
/*2FC*/firmware[191] = 32'h080C4402;
/*300*/firmware[192] = 32'hC13C0200;
/*304*/firmware[193] = 32'h0D44C844;
/*308*/firmware[194] = 32'h00000002;
/*30C*/firmware[195] = 32'h00000010;
/*310*/firmware[196] = 32'h00000000;
/*314*/firmware[197] = 32'h00527A01;
/*318*/firmware[198] = 32'h01017C01;
/*31C*/firmware[199] = 32'h00020D1B;
/*320*/firmware[200] = 32'h00000020;
/*324*/firmware[201] = 32'h00000018;
/*328*/firmware[202] = 32'hFFFFFF34;
/*32C*/firmware[203] = 32'h00000074;
/*330*/firmware[204] = 32'h200E4400;
/*334*/firmware[205] = 32'h44018844;
/*338*/firmware[206] = 32'h0200080C;
/*33C*/firmware[207] = 32'h0D44C860;
/*340*/firmware[208] = 32'h00000002;
/*344*/firmware[209] = 32'h00000000;
/*348*/firmware[210] = 32'h00000000;
/*34C*/firmware[211] = 32'h00000000;
/*350*/firmware[212] = 32'h00000000;
/*354*/firmware[213] = 32'h00000000;
/*358*/firmware[214] = 32'h00000000;
/*35C*/firmware[215] = 32'h00000000;
/*360*/firmware[216] = 32'h00000000;
/*364*/firmware[217] = 32'h00000000;
/*368*/firmware[218] = 32'h00000000;
/*36C*/firmware[219] = 32'h00000000;
/*370*/firmware[220] = 32'h00000000;
/*374*/firmware[221] = 32'h00000000;
/*378*/firmware[222] = 32'h00000000;
/*37C*/firmware[223] = 32'h00000000;
/*380*/firmware[224] = 32'h00000000;
/*384*/firmware[225] = 32'h00000000;
/*388*/firmware[226] = 32'h00000000;
/*38C*/firmware[227] = 32'h00000000;
/*390*/firmware[228] = 32'h00000000;
/*394*/firmware[229] = 32'h00000000;
/*398*/firmware[230] = 32'h00000000;
/*39C*/firmware[231] = 32'h00000000;
/*3A0*/firmware[232] = 32'h00000000;
/*3A4*/firmware[233] = 32'h00000000;
/*3A8*/firmware[234] = 32'h00000000;
/*3AC*/firmware[235] = 32'h00000000;
/*3B0*/firmware[236] = 32'h00000000;
/*3B4*/firmware[237] = 32'h00000000;
/*3B8*/firmware[238] = 32'h00000000;
/*3BC*/firmware[239] = 32'h00000000;
/*3C0*/firmware[240] = 32'h00000000;
/*3C4*/firmware[241] = 32'h00000000;
/*3C8*/firmware[242] = 32'h00000000;
/*3CC*/firmware[243] = 32'h00000000;
/*3D0*/firmware[244] = 32'h00000000;
/*3D4*/firmware[245] = 32'h00000000;
/*3D8*/firmware[246] = 32'h00000000;
/*3DC*/firmware[247] = 32'h00000000;
/*3E0*/firmware[248] = 32'h00000000;
/*3E4*/firmware[249] = 32'h00000000;
/*3E8*/firmware[250] = 32'h00000000;
/*3EC*/firmware[251] = 32'h00000000;
/*3F0*/firmware[252] = 32'h00000000;
/*3F4*/firmware[253] = 32'h00000000;
/*3F8*/firmware[254] = 32'h00000000;
/*3FC*/firmware[255] = 32'h00000000;
/*400*/firmware[256] = 32'h00000000;
/*404*/firmware[257] = 32'h00000000;
/*408*/firmware[258] = 32'h00000000;
/*40C*/firmware[259] = 32'h00000000;
/*410*/firmware[260] = 32'h00000000;
/*414*/firmware[261] = 32'h00000000;
/*418*/firmware[262] = 32'h00000000;
/*41C*/firmware[263] = 32'h00000000;
/*420*/firmware[264] = 32'h00000000;
/*424*/firmware[265] = 32'h00000000;
/*428*/firmware[266] = 32'h00000000;
/*42C*/firmware[267] = 32'h00000000;
/*430*/firmware[268] = 32'h00000000;
/*434*/firmware[269] = 32'h00000000;
/*438*/firmware[270] = 32'h00000000;
/*43C*/firmware[271] = 32'h00000000;
/*440*/firmware[272] = 32'h00000000;
/*444*/firmware[273] = 32'h00000000;
/*448*/firmware[274] = 32'h00000000;
/*44C*/firmware[275] = 32'h00000000;
/*450*/firmware[276] = 32'h00000000;
/*454*/firmware[277] = 32'h00000000;
/*458*/firmware[278] = 32'h00000000;
/*45C*/firmware[279] = 32'h00000000;
/*460*/firmware[280] = 32'h00000000;
/*464*/firmware[281] = 32'h00000000;
/*468*/firmware[282] = 32'h00000000;
/*46C*/firmware[283] = 32'h00000000;
/*470*/firmware[284] = 32'h00000000;
/*474*/firmware[285] = 32'h00000000;
/*478*/firmware[286] = 32'h00000000;
/*47C*/firmware[287] = 32'h00000000;
/*480*/firmware[288] = 32'h00000000;
/*484*/firmware[289] = 32'h00000000;
/*488*/firmware[290] = 32'h00000000;
/*48C*/firmware[291] = 32'h00000000;
/*490*/firmware[292] = 32'h00000000;
/*494*/firmware[293] = 32'h00000000;
/*498*/firmware[294] = 32'h00000000;
/*49C*/firmware[295] = 32'h00000000;
/*4A0*/firmware[296] = 32'h00000000;
/*4A4*/firmware[297] = 32'h00000000;
/*4A8*/firmware[298] = 32'h00000000;
/*4AC*/firmware[299] = 32'h00000000;
/*4B0*/firmware[300] = 32'h00000000;
/*4B4*/firmware[301] = 32'h00000000;
/*4B8*/firmware[302] = 32'h00000000;
/*4BC*/firmware[303] = 32'h00000000;
/*4C0*/firmware[304] = 32'h00000000;
/*4C4*/firmware[305] = 32'h00000000;
/*4C8*/firmware[306] = 32'h00000000;
/*4CC*/firmware[307] = 32'h00000000;
/*4D0*/firmware[308] = 32'h00000000;
/*4D4*/firmware[309] = 32'h00000000;
/*4D8*/firmware[310] = 32'h00000000;
/*4DC*/firmware[311] = 32'h00000000;
/*4E0*/firmware[312] = 32'h00000000;
/*4E4*/firmware[313] = 32'h00000000;
/*4E8*/firmware[314] = 32'h00000000;
/*4EC*/firmware[315] = 32'h00000000;
/*4F0*/firmware[316] = 32'h00000000;
/*4F4*/firmware[317] = 32'h00000000;
/*4F8*/firmware[318] = 32'h00000000;
/*4FC*/firmware[319] = 32'h00000000;
/*500*/firmware[320] = 32'h00000000;
/*504*/firmware[321] = 32'h00000000;
/*508*/firmware[322] = 32'h00000000;
/*50C*/firmware[323] = 32'h00000000;
/*510*/firmware[324] = 32'h00000000;
/*514*/firmware[325] = 32'h00000000;
/*518*/firmware[326] = 32'h00000000;
/*51C*/firmware[327] = 32'h00000000;
/*520*/firmware[328] = 32'h00000000;
/*524*/firmware[329] = 32'h00000000;
/*528*/firmware[330] = 32'h00000000;
/*52C*/firmware[331] = 32'h00000000;
/*530*/firmware[332] = 32'h00000000;
/*534*/firmware[333] = 32'h00000000;
/*538*/firmware[334] = 32'h00000000;
/*53C*/firmware[335] = 32'h00000000;
/*540*/firmware[336] = 32'h00000000;
/*544*/firmware[337] = 32'h00000000;
/*548*/firmware[338] = 32'h00000000;
/*54C*/firmware[339] = 32'h00000000;
/*550*/firmware[340] = 32'h00000000;
/*554*/firmware[341] = 32'h00000000;
/*558*/firmware[342] = 32'h00000000;
/*55C*/firmware[343] = 32'h00000000;
/*560*/firmware[344] = 32'h00000000;
/*564*/firmware[345] = 32'h00000000;
/*568*/firmware[346] = 32'h00000000;
/*56C*/firmware[347] = 32'h00000000;
/*570*/firmware[348] = 32'h00000000;
/*574*/firmware[349] = 32'h00000000;
/*578*/firmware[350] = 32'h00000000;
/*57C*/firmware[351] = 32'h00000000;
/*580*/firmware[352] = 32'h00000000;
/*584*/firmware[353] = 32'h00000000;
/*588*/firmware[354] = 32'h00000000;
/*58C*/firmware[355] = 32'h00000000;
/*590*/firmware[356] = 32'h00000000;
/*594*/firmware[357] = 32'h00000000;
/*598*/firmware[358] = 32'h00000000;
/*59C*/firmware[359] = 32'h00000000;
/*5A0*/firmware[360] = 32'h00000000;
/*5A4*/firmware[361] = 32'h00000000;
/*5A8*/firmware[362] = 32'h00000000;
/*5AC*/firmware[363] = 32'h00000000;
/*5B0*/firmware[364] = 32'h00000000;
/*5B4*/firmware[365] = 32'h00000000;
/*5B8*/firmware[366] = 32'h00000000;
/*5BC*/firmware[367] = 32'h00000000;
/*5C0*/firmware[368] = 32'h00000000;
/*5C4*/firmware[369] = 32'h00000000;
/*5C8*/firmware[370] = 32'h00000000;
/*5CC*/firmware[371] = 32'h00000000;
/*5D0*/firmware[372] = 32'h00000000;
/*5D4*/firmware[373] = 32'h00000000;
/*5D8*/firmware[374] = 32'h00000000;
/*5DC*/firmware[375] = 32'h00000000;
/*5E0*/firmware[376] = 32'h00000000;
/*5E4*/firmware[377] = 32'h00000000;
/*5E8*/firmware[378] = 32'h00000000;
/*5EC*/firmware[379] = 32'h00000000;
/*5F0*/firmware[380] = 32'h00000000;
/*5F4*/firmware[381] = 32'h00000000;
/*5F8*/firmware[382] = 32'h00000000;
/*5FC*/firmware[383] = 32'h00000000;
/*600*/firmware[384] = 32'h00000000;
/*604*/firmware[385] = 32'h00000000;
/*608*/firmware[386] = 32'h00000000;
/*60C*/firmware[387] = 32'h00000000;
/*610*/firmware[388] = 32'h00000000;
/*614*/firmware[389] = 32'h00000000;
/*618*/firmware[390] = 32'h00000000;
/*61C*/firmware[391] = 32'h00000000;
/*620*/firmware[392] = 32'h00000000;
/*624*/firmware[393] = 32'h00000000;
/*628*/firmware[394] = 32'h00000000;
/*62C*/firmware[395] = 32'h00000000;
/*630*/firmware[396] = 32'h00000000;
/*634*/firmware[397] = 32'h00000000;
/*638*/firmware[398] = 32'h00000000;
/*63C*/firmware[399] = 32'h00000000;
/*640*/firmware[400] = 32'h00000000;
/*644*/firmware[401] = 32'h00000000;
/*648*/firmware[402] = 32'h00000000;
/*64C*/firmware[403] = 32'h00000000;
/*650*/firmware[404] = 32'h00000000;
/*654*/firmware[405] = 32'h00000000;
/*658*/firmware[406] = 32'h00000000;
/*65C*/firmware[407] = 32'h00000000;
/*660*/firmware[408] = 32'h00000000;
/*664*/firmware[409] = 32'h00000000;
/*668*/firmware[410] = 32'h00000000;
/*66C*/firmware[411] = 32'h00000000;
/*670*/firmware[412] = 32'h00000000;
/*674*/firmware[413] = 32'h00000000;
/*678*/firmware[414] = 32'h00000000;
/*67C*/firmware[415] = 32'h00000000;
/*680*/firmware[416] = 32'h00000000;
/*684*/firmware[417] = 32'h00000000;
/*688*/firmware[418] = 32'h00000000;
/*68C*/firmware[419] = 32'h00000000;
/*690*/firmware[420] = 32'h00000000;
/*694*/firmware[421] = 32'h00000000;
/*698*/firmware[422] = 32'h00000000;
/*69C*/firmware[423] = 32'h00000000;
/*6A0*/firmware[424] = 32'h00000000;
/*6A4*/firmware[425] = 32'h00000000;
/*6A8*/firmware[426] = 32'h00000000;
/*6AC*/firmware[427] = 32'h00000000;
/*6B0*/firmware[428] = 32'h00000000;
/*6B4*/firmware[429] = 32'h00000000;
/*6B8*/firmware[430] = 32'h00000000;
/*6BC*/firmware[431] = 32'h00000000;
/*6C0*/firmware[432] = 32'h00000000;
/*6C4*/firmware[433] = 32'h00000000;
/*6C8*/firmware[434] = 32'h00000000;
/*6CC*/firmware[435] = 32'h00000000;
/*6D0*/firmware[436] = 32'h00000000;
/*6D4*/firmware[437] = 32'h00000000;
/*6D8*/firmware[438] = 32'h00000000;
/*6DC*/firmware[439] = 32'h00000000;
/*6E0*/firmware[440] = 32'h00000000;
/*6E4*/firmware[441] = 32'h00000000;
/*6E8*/firmware[442] = 32'h00000000;
/*6EC*/firmware[443] = 32'h00000000;
/*6F0*/firmware[444] = 32'h00000000;
/*6F4*/firmware[445] = 32'h00000000;
/*6F8*/firmware[446] = 32'h00000000;
/*6FC*/firmware[447] = 32'h00000000;
/*700*/firmware[448] = 32'h00000000;
/*704*/firmware[449] = 32'h00000000;
/*708*/firmware[450] = 32'h00000000;
/*70C*/firmware[451] = 32'h00000000;
/*710*/firmware[452] = 32'h00000000;
/*714*/firmware[453] = 32'h00000000;
/*718*/firmware[454] = 32'h00000000;
/*71C*/firmware[455] = 32'h00000000;
/*720*/firmware[456] = 32'h00000000;
/*724*/firmware[457] = 32'h00000000;
/*728*/firmware[458] = 32'h00000000;
/*72C*/firmware[459] = 32'h00000000;
/*730*/firmware[460] = 32'h00000000;
/*734*/firmware[461] = 32'h00000000;
/*738*/firmware[462] = 32'h00000000;
/*73C*/firmware[463] = 32'h00000000;
/*740*/firmware[464] = 32'h00000000;
/*744*/firmware[465] = 32'h00000000;
/*748*/firmware[466] = 32'h00000000;
/*74C*/firmware[467] = 32'h00000000;
/*750*/firmware[468] = 32'h00000000;
/*754*/firmware[469] = 32'h00000000;
/*758*/firmware[470] = 32'h00000000;
/*75C*/firmware[471] = 32'h00000000;
/*760*/firmware[472] = 32'h00000000;
/*764*/firmware[473] = 32'h00000000;
/*768*/firmware[474] = 32'h00000000;
/*76C*/firmware[475] = 32'h00000000;
/*770*/firmware[476] = 32'h00000000;
/*774*/firmware[477] = 32'h00000000;
/*778*/firmware[478] = 32'h00000000;
/*77C*/firmware[479] = 32'h00000000;
/*780*/firmware[480] = 32'h00000000;
/*784*/firmware[481] = 32'h00000000;
/*788*/firmware[482] = 32'h00000000;
/*78C*/firmware[483] = 32'h00000000;
/*790*/firmware[484] = 32'h00000000;
/*794*/firmware[485] = 32'h00000000;
/*798*/firmware[486] = 32'h00000000;
/*79C*/firmware[487] = 32'h00000000;
/*7A0*/firmware[488] = 32'h00000000;
/*7A4*/firmware[489] = 32'h00000000;
/*7A8*/firmware[490] = 32'h00000000;
/*7AC*/firmware[491] = 32'h00000000;
/*7B0*/firmware[492] = 32'h00000000;
/*7B4*/firmware[493] = 32'h00000000;
/*7B8*/firmware[494] = 32'h00000000;
/*7BC*/firmware[495] = 32'h00000000;
/*7C0*/firmware[496] = 32'h00000000;
/*7C4*/firmware[497] = 32'h00000000;
/*7C8*/firmware[498] = 32'h00000000;
/*7CC*/firmware[499] = 32'h00000000;
/*7D0*/firmware[500] = 32'h00000000;
/*7D4*/firmware[501] = 32'h00000000;
/*7D8*/firmware[502] = 32'h00000000;
/*7DC*/firmware[503] = 32'h00000000;
/*7E0*/firmware[504] = 32'h00000000;
/*7E4*/firmware[505] = 32'h00000000;
/*7E8*/firmware[506] = 32'h00000000;
/*7EC*/firmware[507] = 32'h00000000;
/*7F0*/firmware[508] = 32'h00000000;
/*7F4*/firmware[509] = 32'h00000000;
/*7F8*/firmware[510] = 32'h00000000;
/*7FC*/firmware[511] = 32'h00000000;

// END MEM_INIT
end

	
endmodule