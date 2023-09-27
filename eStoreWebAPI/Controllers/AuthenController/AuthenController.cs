using AutoMapper;
using BusinessObject;
using eStoreWebAPI.DTO.Members;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Services.Members;

namespace eStoreWebAPI.Controllers.AuthenController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public AuthenController(IMemberRepository memberRepository, IMapper mapper, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
    

        [HttpPost("Login")]
        public ActionResult Login(LoginDTO l)
        {
            /*var login = _mapper.Map<MemberDTO>(l);*/
            var member = _mapper.Map<MemberDTO>(_memberRepository.Login(l.Email, l.Password));
            if (member == null) return BadRequest("Incorrect email or password!");
            return Ok(member);
        }



        [HttpPost("SignUp")]
        public ActionResult SignUp(SignUpDTO m)
        {
            string adminEmail = this._configuration.GetValue<string>("Account:email");
            string adminPassword = this._configuration.GetValue<string>("Account:password");
            var member = _mapper.Map<Member>(m);
            if (m.Email == adminEmail || m.Password == adminPassword) {
                return BadRequest("Can't register this account!");
            }
            if (_memberRepository.RegisterOrNot(m.Email))
            {
                return BadRequest("Email is exist!!!");
            }
            if (ModelState.IsValid) { 
            _memberRepository.InsertMember(member);
            }

            return Ok(member);
      
        }
    }
}
