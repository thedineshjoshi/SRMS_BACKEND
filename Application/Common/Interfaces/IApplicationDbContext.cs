﻿using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<StudentRegistration> StudentRegistrations { get; }

        Task <int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
