using AutoMapper;
using BusinessObject;
using eStoreWebAPI.DTO.Members;
using Microsoft.AspNetCore.Mvc;
using Repository.Services.Members;
using System.Collections.Generic;

namespace eStoreWebAPI.Controllers.MemberController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Controller
    {

        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberController(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetMembers()
        {
            var m = _memberRepository.GetMembers();
            var mDTO = _mapper.Map<IEnumerable<MemberDTO>>(m);
            return Ok(mDTO);
        }

        [HttpGet("{id}")]
        public ActionResult FindMemberById(int id)
        {
            var m = _memberRepository.GetMemberById(id);
            var mDTO = _mapper.Map<MemberDTO>(m);
            return Ok(mDTO);
        }

        [HttpPost]
        public ActionResult<MemberDTO> SaveMember(MemberDTO m)
        {
            var member = _mapper.Map<Member>(m);

            if (_memberRepository.RegisterOrNot(m.Email))
            {
                return BadRequest("Email is exist!!!");
            }
        

            _memberRepository.InsertMember(member);
            return Ok(member);

        }

        [HttpPost("{id}")]
        public ActionResult UpdateMember(int id,SignUpDTO m)
        {
            var mId = _memberRepository.GetMemberById(id);

            if (mId == null)
                return NotFound();
            var mem = _mapper.Map(m, mId);
            _memberRepository.UpdateMember(mem);
            return Ok(mem);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMember(int id)
        {
            var member = _memberRepository.GetMemberById(id);
            if (member == null)
                return NotFound();
            _memberRepository.DeleteMember(member);
            return NoContent();
        }
    }
}
