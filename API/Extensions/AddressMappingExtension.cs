using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Core.Entities;

namespace API.Error
{
    public static class AddressMappingExtension
    {
        public static AddressDto? ToDto (this Address? address) {
            if (address == null) return null;
            return new AddressDto {
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                State = address.State
                
            };
        }

        public static Address? ToEntity (this AddressDto? address) {
            if (address == null) return null;
            return new Address {
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                State = address.State
            };
        }

        public static void UpdateAddress( this Address address, AddressDto addressDto) {
            if (addressDto == null) throw new ArgumentNullException(nameof(addressDto));
            if (address == null) throw new ArgumentNullException(nameof(address));
                address.Line1 = addressDto.Line1;
                address.Line2 = addressDto.Line2;
                address.City = addressDto.City;
                address.Country = addressDto.Country;
                address.PostalCode = addressDto.PostalCode;
                address.State = addressDto.State;
        }


    }
}