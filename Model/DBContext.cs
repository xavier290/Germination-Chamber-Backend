﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ModelsMQTT_Server
{
    public class MqttDbContext : IdentityDbContext<User>
    {
        public MqttDbContext(DbContextOptions<MqttDbContext> options) : base(options) { }

        public DbSet<Temperature> Temperature { get; set; }
    }
}