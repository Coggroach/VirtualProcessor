# VirtualProcessor
Arm like Processor

#General Info:

4 Register Files for each Mode
16 Registers per File (r0-r15)
r15 is Dedicated TempRegister
No Access to Program Counter Except in System and Interrupt

Constants - 4 Bits Max (0 - 15)

Commmand (DestinationRegister) (OperatorA) (OperatorB)

CMD rx, ry, #1

Keywords
sp = StackPointer = r13
lr = LinkRegister = r14
tr = TempRegister = r15
MOD System
MOD Interrupt
MOD Previlaged
MOD UnPrevilaged

#Commands:

LDR		- Load Register 
LDR r0, =1400; LDR r0, r1
LDRC	- Load Register with Constant 
LDRC r0, #14
STR 	- Store Register (Store's r0's Content at Memory Location r1)
STR r0, [r1]

//Arithmetic Flags
INC		- Increment Reg 		
INC r0
ADD		- Add Reg 				
ADD r0, r2, r1; ADD r1, r2, #2
ADDI	- Add With Increment 	
ADDI r0, r1, r2; ADDI r0, r1, #1
ADC		- Add With Carry Flag 	
ADC r0, r1, r2; ADC r0, r1, #1
SUBD	- Sub With Decrement 	
SUBD r0, r1, r2; SUBD r0, r1, #1
SUB		- Sub Reg 				
SUB r0, r2, r1; SUB r0, r2, #1
SBC		- Sub With Carry Flag	
SBC r0, r1, r2; SBC r0, r2, #2
DEC		- Decrement Reg			
DEC r1
RSB		- 

RSC		-

//Multiplication Flags
MUL		- Multiply Registers	
MUL r0, r2, r2; MUL r0, r2, #2
MLA		- 

//Logical Flags
AND		- AND Registers			
AND r0, r2, r1; AND r0, r1, #1
EOR		- EOR Registers			
EOR r0, r2, r1; EOR r0, r2, #3
ORR		- OR Registers			
ORR r0, r2, r1; ORR r1, r2, #7
BIC		- Bit Clear				
BIC r1, r2, r1; BIC r3, r4, r1

//Data Movement
MOV		- Mov Data to Register	
MOV r0, #1; MOV r0, r1
MNV		- Mov Not Data			
MNV r0, r1; MOV r1, #2

//Shifting Flags
ROL		- Roll Shift Left		
ROL r0, r1, #1; ROL r2, r2, r1
ROR		- Roll Shift Right		
ROR r0, r1, #2; ROR r1, r2, r1
LSL 	- Logical Shift Left	
LSL r0, r1, r2; LSL r9, r2, #1
LSR		- Logical Shift Right	
LSR r0, r2, r1; LSR r0, r1, #2

//Comparison Flags
TST		- 
TEQ		-
CMN		- Update Status rx+ry	
CMN r0, r1; CMN r0, #0
CMP		- Update Status rx-ry	
CMP r0, r1; CMP r1, #1

//ProgramControl
LDPC	- Load From Pc To Register		
LDPC r13
STPC	- Store To Pc From Register		
STPC r12
MOD		- Change Datapath Mode			
MOD System
EOI		- End of Interrupt/Initialize Interrupts	
EOI

//Compound Commands
LDM		- Load Register(s) from Address		
LDM sp! {r0, r2, r1}
STM		- Store Register(s) to Address		
STM sp! {r1, r2, r0}
BX		- Branch Subroutine					
BX subroutine
^		- Return From Subroutine			
^