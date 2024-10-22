using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Buffers.Text;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Runtime.InteropServices;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using NZWalks.API.Repositories.Interface;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public RegionRepository( NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
             await _nZWalksDbContext.AddAsync(region);
           await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);


            //check if it's null first 
            if (region == null)
            {
                return null;
            }
            //then
            //Delete the data
            _nZWalksDbContext.Regions.Remove(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        //  GetAllAsync() Method:

        //This method fetches all regions from the database.
        //It returns a collection of regions (IEnumerable<Region>).
        //In SQL, it would correspond to a query like: SELECT* FROM Regions;
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
           return await _nZWalksDbContext.Regions.ToListAsync();
        }


               // GetAsync(Guid Id) Method:

             //This method fetches a region from the database based on its unique identifier(Id).
             //It returns a single region(Region object) that matches the provided Id.
          //In SQL, it would correspond to a query like: SELECT* FROM Regions WHERE Id = [provided Id];
        public async Task<Region> GetAsync(Guid Id)
        {
          return  await  _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
                 //Fetching the Existing Region from the Database:

                 //The code starts by searching for an existing region in the database based on the provided id.
                 //In SQL terms, it's similar to executing a query like: SELECT * FROM Regions WHERE Id = [provided id];

         //Here, _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id) finds the first region that matches the provided id.
           var existingregion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);


            //Checking if the Existing Region Exists:

        //The code checks if the existingRegion variable is null.
     //If it is, it means there's no region with the provided id in the database.
   //This part is like a validation step. If the region doesn't exist,
   //it returns null to indicate that the update operation couldn't be performed.
            if (existingregion == null)
            {
                return null;
            }

                 //Updating the Existing Region:

           //If the existingRegion is found(i.e., it's not null),
        //the code updates its properties with the values from the region object provided as a parameter to the method.
//It updates properties like: Code, Name, Area, Lat, Long, and Population with the new values provided in the region object.
//This part doesn't directly correspond to SQL operations.
//Instead, it modifies the properties of the existingRegion object in memory.


            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.Area = region.Area;
            existingregion.Lat = region.Lat;
            existingregion.Long = region.Long;
            existingregion.Population = region.Population;

          //Saving Changes to the Database:

        //After updating the properties of the existingRegion object,
        //the code calls _nZWalksDbContext.SaveChangesAsync() to save these changes to the database.
        //In SQL terms, this is like executing an UPDATE statement to modify the existing record in the Regions table with the new values.
            await _nZWalksDbContext.SaveChangesAsync();
            return existingregion;
        }
    }
}
