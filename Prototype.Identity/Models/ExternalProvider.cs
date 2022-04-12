// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Notice: Modified by Joshua Abbott

using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Identity.Models
{
    [NotMapped]
    public class ExternalProvider
    {
        public string? DisplayName { get; set; }
        public string? AuthenticationScheme { get; set; }
    }
}
