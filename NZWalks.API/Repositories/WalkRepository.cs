﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign New ID
            walk.Id = Guid.NewGuid();
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {

            var existingWalk = await _nZWalksDbContext.Walks.FindAsync(id);

            //check if it's null first 
            if (existingWalk == null)
            {
                return null;
            }

            // then Delete the data
            _nZWalksDbContext.Walks.Remove(existingWalk);
            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalk; ;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _nZWalksDbContext.Walks
                 .Include(x => x.Region)
                 .Include(x => x.WalkDifficulty)
                 .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return _nZWalksDbContext.Walks
                .Include(x => x.WalkDifficulty)
                .Include(x => x.Region)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _nZWalksDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;

            }
            existingWalk.Length = walk.Length;
            existingWalk.Name = walk.Name;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
