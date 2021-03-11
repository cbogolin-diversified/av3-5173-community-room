C2I-DMPS3-4K-350-VIDEO>pnmadddmz ?

PNMADDDMZ  -T:{vlantype} -S:{slotType} -P:{slotNumber}

-T: specifies the 0-based VLAN type:

  0 - customer VLAN

  1 - Crestron control VLAN

  2 - Crestron streaming VLAN

  3 - Crestron data VLAN

  4 - Crestron streaming_1 VLAN

-S: specifies the 0-based Slot type

  0 - dminput slot

  1 - dmoutput slot

  2 - dmstream slot

  3 - dmcontent slot

-P: specifies the 1-based slot number

C2I-DMPS3-4K-350-VIDEO>PNMADDDMZ -T:3 -S:1 -P:13

OpCard Slot 13 added to VLAN3 DMZ. Reboot to Take Effect

C2I-DMPS3-4K-350-VIDEO>PNMADDDMZ -T:3 -S:1 -P:14

OpCard Slot 14 added to VLAN3 DMZ. Reboot to Take Effect

C2I-DMPS3-4K-350-VIDEO>

DMPS3-4K-350-C>reboot
