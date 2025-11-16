
using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_mangment.library.domain.@interface;
using library_mangment.library.Infrastructure.@interface;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace library_task.library.Application.services
{
    public class MemberService : IMemberService

    {
        private readonly IMemberRepo MemberRepo;
        private readonly IauthService authService;

        public MemberService(IMemberRepo MemberRepo, IauthService authService)
        {
            this.MemberRepo = MemberRepo;
            this.authService = authService;
        }
        public ICollection<MemberDto> GetAllMembers()
        {
            var members = MemberRepo.Getmembers();
            var memberDtos = members.Select(b => new MemberDto
            {
                Name = b.Name,
                Email = b.Email
            }).ToList();
            return memberDtos;
        }

        public member? GetById(int id)
        {
            return MemberRepo.GetMemberByID(id);
        }

        public string? Login(logindto dto)
        {
            var existingMember = MemberRepo.GetMemberByEmail(dto.Email);
            if (existingMember == null)
            {
                Console.WriteLine($"Login failed: No member with email {dto.Email}");
                return string.Empty;
            }

            var hasher = new PasswordHasher<member>();
            var result = hasher.VerifyHashedPassword(existingMember, existingMember.passwordHas, dto.password);

            
            if (result == PasswordVerificationResult.Failed)
            {
                Console.WriteLine($"Login failed: Invalid password for {dto.Email}");
                return string.Empty;
            }

            
            var token = authService.createtoken(existingMember);
            Console.WriteLine($"Login successful for {dto.Email}, token generated");
            return token;
        }

        public bool Register(MemberDto dto)
        {
            var existingMember = MemberRepo.GetMemberByEmail(dto.Email);
            if (existingMember != null)
            {
                Console.WriteLine($"Registration failed: Email {dto.Email} already exists");
                return false;
            }

            var newMember = new member
            {
                Name = dto.Name,
                Email = dto.Email,
                JoinDate = DateTime.Now,
                ImageUrl = dto.ImageUrl ?? null,
                role ="member",
            };

            
            var hasher = new PasswordHasher<member>();
            newMember.passwordHas = hasher.HashPassword(newMember, dto.password);

            var result = MemberRepo.CreatMember(newMember);
            Console.WriteLine($"Registration {(result ? "successful" : "failed")} for {dto.Email}");
            return result;
        }
        public bool UpdateMemberInfo(MemberDto dto)
        {
            var existingMember = MemberRepo.GetMemberByID(dto.Id);
            if (existingMember == null)
                return false;

           
            existingMember.Name = dto.Name ?? existingMember.Name;
            existingMember.Email = dto.Email ?? existingMember.Email;

          
            if (!string.IsNullOrEmpty(dto.password))
            {
                var hasher = new PasswordHasher<member>();
                existingMember.passwordHas = hasher.HashPassword(existingMember, dto.password);
            }

            existingMember.ImageUrl = dto.ImageUrl ?? existingMember.ImageUrl;

            return MemberRepo.UpdateMember(existingMember);
        }
    }
}
