﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class RegisterPlayerCommand : Command<RegisterPlayerResponse>
    {
        #region Properties

        /// <summary>
        /// Gets the register player request.
        /// </summary>
        /// <value>
        /// The register player request.
        /// </value>
        public RegisterPlayerRequest RegisterPlayerRequest { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Gets the add measured course to club request.
        /// </summary>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <param name="commandId">The command identifier.</param>
        /// <value>
        /// The add measured course to club request.
        /// </value>
        private RegisterPlayerCommand(RegisterPlayerRequest registerPlayerRequest, Guid commandId) : base(commandId)
        {
            this.RegisterPlayerRequest = registerPlayerRequest;
        }
        #endregion

        #region public static RegisterPlayerCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <returns></returns>
        public static RegisterPlayerCommand Create(RegisterPlayerRequest registerPlayerRequest)
        {
            return new RegisterPlayerCommand(registerPlayerRequest, Guid.NewGuid());
        }
        #endregion
    }
}