﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            //Assign New ID
            walkDifficulty.Id = Guid.NewGuid();
            await _nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await _nZWalksDbContext.WalkDifficulty.FindAsync(id);

            //check if it's null first 
            if (existingWalkDifficulty == null)
            {
                return null;
            }

            // then Delete the data
            _nZWalksDbContext.WalkDifficulty.Remove(existingWalkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty; ;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _nZWalksDbContext.WalkDifficulty.ToListAsync();


        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            //return await _nZWalksDbContext.WalkDifficulty.FindAsync(id);
            return await _nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _nZWalksDbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;

            }
            existingWalkDifficulty.Code = walkDifficulty.Code;
            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
