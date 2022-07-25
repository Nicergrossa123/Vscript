﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Logging;

namespace Nexus.Module.Tattoo
{
    public class TattooLicense : Loadable<uint>
    {
        public uint Id { get; }

        public uint AssetsTattooId { get; }

        public int Price { get; }

        public AssetsTattoo Tattoo { get; }

        public TattooLicense(MySqlDataReader reader) : base(reader)
        {
            Id = reader.GetUInt32("tattooLicenseId");
            AssetsTattooId = reader.GetUInt32("assets_tattoo_id");
            Price = reader.GetInt32("tattooLicensePrice");

            try
            {
                Tattoo = new AssetsTattoo(reader);
            }
            catch (Exception e)
            {
                Logger.Crash(e);
            }
        }

        public override uint GetIdentifier()
        {
            return Id;
        }
    }
}