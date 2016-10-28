﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V6_0_2_19033.Parsers
{
    public static class GroupHandler
    {
        [Parser(Opcode.SMSG_PARTY_MEMBER_STATE)]
        public static void HandlePartyMemberState(Packet packet)
        {
            packet.ReadBit("ForEnemy");

            for (var i = 0; i < 2; i++)
                packet.ReadByte("PartyType", i);

            packet.ReadInt16E<GroupMemberStatusFlag>("Flags");

            packet.ReadByte("PowerType");
            packet.ReadInt16("OverrideDisplayPower");
            packet.ReadInt32("CurrentHealth");
            packet.ReadInt32("MaxHealth");
            packet.ReadInt16("MaxPower");
            packet.ReadInt16("MaxPower");
            packet.ReadInt16("Level");
            packet.ReadInt16("Spec");
            packet.ReadInt16("AreaID");

            packet.ReadInt16("WmoGroupID");
            packet.ReadInt32("WmoDoodadPlacementID");

            packet.ReadInt16("PositionX");
            packet.ReadInt16("PositionY");
            packet.ReadInt16("PositionZ");

            packet.ReadInt32("VehicleSeatRecID");
            var auraCount = packet.ReadInt32("AuraCount");

            packet.ReadInt32("PhaseShiftFlags");
            var int4 = packet.ReadInt32("PhaseCount");
            packet.ReadPackedGuid128("PersonalGUID");
            for (int i = 0; i < int4; i++)
            {
                packet.ReadInt16("PhaseFlags", i);
                packet.ReadInt16("Id", i);
            }

            for (int i = 0; i < auraCount; i++)
            {
                packet.ReadInt32<SpellId>("Aura", i);
                packet.ReadByte("Flags", i);
                packet.ReadInt32("ActiveFlags", i);
                var byte3 = packet.ReadInt32("PointsCount", i);

                for (int j = 0; j < byte3; j++)
                    packet.ReadSingle("Points", i, j);
            }

            packet.ResetBitReader();

            var hasPet = packet.ReadBit("HasPet");
            if (hasPet) // Pet
            {
                packet.ReadPackedGuid128("PetGuid");
                packet.ReadInt16("PetDisplayID");
                packet.ReadInt32("PetMaxHealth");
                packet.ReadInt32("PetHealth");

                var petAuraCount = packet.ReadInt32("PetAuraCount");
                for (int i = 0; i < petAuraCount; i++)
                {
                    packet.ReadInt32<SpellId>("PetAura", i);
                    packet.ReadByte("PetFlags", i);
                    packet.ReadInt32("PetActiveFlags", i);
                    var byte3 = packet.ReadInt32("PetPointsCount", i);

                    for (int j = 0; j < byte3; j++)
                        packet.ReadSingle("PetPoints", i, j);
                }

                packet.ResetBitReader();

                var len = packet.ReadBits(8);
                packet.ReadWoWString("PetName", len);
            }

            packet.ReadPackedGuid128("MemberGuid");
        }

        [Parser(Opcode.CMSG_PARTY_INVITE, ClientVersionBuild.V7_1_0_22900)]
        public static void HandleClientPartyInvite(Packet packet)
        {
            packet.ReadByte("PartyIndex");
            packet.ReadInt32("ProposedRoles");
            packet.ReadPackedGuid128("TargetGuid");

            packet.ResetBitReader();

            var lenTargetName = packet.ReadBits(9);
            var lenTargetRealm = packet.ReadBits(9);

            packet.ReadWoWString("TargetName", lenTargetName);
            packet.ReadWoWString("TargetRealm", lenTargetRealm);
        }

        [Parser(Opcode.SMSG_PARTY_UPDATE, ClientVersionBuild.V7_1_0_22900)]
        public static void HandlePartyUpdate(Packet packet)
        {
            packet.ReadByte("PartyFlags");
            packet.ReadByte("PartyIndex");
            packet.ReadByte("PartyType");

            packet.ReadInt32("MyIndex");
            packet.ReadPackedGuid128("LeaderGUID");
            packet.ReadInt32("SequenceNum");
            packet.ReadPackedGuid128("PartyGUID");

            var int13 = packet.ReadInt32("PlayerListCount");
            for (int i = 0; i < int13; i++)
            {
                packet.ResetBitReader();
                var bits76 = packet.ReadBits(6);
                packet.ReadBit("FromSocialQueue");

                packet.ReadPackedGuid128("Guid", i);

                packet.ReadByte("Connected", i);
                packet.ReadByte("Subgroup", i);
                packet.ReadByte("Flags", i);
                packet.ReadByte("RolesAssigned", i);
                packet.ReadByteE<Class>("PlayerClass", i);

                packet.ReadWoWString("Name", bits76, i);
            }

            packet.ResetBitReader();

            var bit68 = packet.ReadBit("HasLfgInfo");
            var bit144 = packet.ReadBit("HasLootSettings");
            var bit164 = packet.ReadBit("HasDifficultySettings");

            if (bit68)
            {
                packet.ReadByte("MyLfgFlags");
                packet.ReadInt32("LfgSlot");
                packet.ReadInt32("MyLfgRandomSlot");
                packet.ReadByte("MyLfgPartialClear");
                packet.ReadSingle("MyLfgGearDiff");
                packet.ReadByte("MyLfgStrangerCount");
                packet.ReadByte("MyLfgKickVoteCount");
                packet.ReadByte("LfgBootCount");

                packet.ResetBitReader();

                packet.ReadBit("LfgAborted");
                packet.ReadBit("MyLfgFirstReward");
            }

            if (bit144)
            {
                packet.ReadByte("LootMethod");
                packet.ReadPackedGuid128("LootMaster");
                packet.ReadByte("LootThreshold");
            }

            if (bit164)
            {
                packet.ReadInt32("Unk Int4");
                //for (int i = 0; i < 2; i++)
                //{
                packet.ReadInt32("DungeonDifficultyID");
                packet.ReadInt32("RaidDifficultyID");
                //}
            }
        }
    }
}