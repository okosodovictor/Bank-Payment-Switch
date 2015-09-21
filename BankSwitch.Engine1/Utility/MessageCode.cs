using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Engine.Utility
{
   public class MessageCode
    {
       public static string fep = "FEP";
       public static string source = "source";
       public static string DefaultFee = "C000000";

       #region//Transaction Response Code TrnxResponse
       public readonly static string TrnxResponse_ApprovedOrcompletedSuccessfully_00 = "00";
       public readonly static string TrnxResponse_ReferToCardIssuer_01 = "01";
       public readonly static string TrnxResponse_ReferToCardIssuerSpecialCondition_02 = "02";
       public readonly static string TrnxResponse_InvalidMerchant_03 = "03";
       public readonly static string TrnxResponse_PickUpCard_04 = "04";
       public readonly static string TrnxResponse_DoNotHonor_05 = "05";
       public readonly static string TrnxResponse_Error_06 = "06";
       public readonly static string TrnxResponse_PickUpCardSpecialCondition_07 = "07";
       public readonly static string TrnxResponse_HonorWithID_08 = "08";
       public readonly static string TrnxResponse_RequestInProgress_09 = "09";
       public readonly static string TrnxResponse_ApprovedPartial_10 = "10";
       public readonly static string TrnxResponse_ApprovedVIP_11 = "11";
       public readonly static string TrnxResponse_InvalidTransaction_12 = "12";
       public readonly static string TrnxResponse_InvalidAmount_13 = "13";
       public readonly static string TrnxResponse_InvalidCardNo_14 = "14";
       public readonly static string TrnxResponse_NoSuchIssuer_15 = "15";
       public readonly static string TrnxResponse_ApprovedUpdateTrack3_16 = "16";
       public readonly static string TrnxResponse_CustomerCancellation_17 = "17";
       public readonly static string TrnxResponse_CustomerDispute_18 = "18";
       public readonly static string TrnxResponse_ReEnterTransaction_19 = "19";
       public readonly static string TrnxResponse_InvalidResponse_20 = "20";
       public readonly static string TrnxResponse_NoActionTaken_21 = "21";
       public readonly static string TrnxResponse_SuspectedMalfunction = "22";
       public readonly static string TrnxResponse_UnacceptableTransactionFee_23 = "23";
       public readonly static string TrnxResponse_FilesUpdateNotSupported_24 = "24";
       public readonly static string TrnxResponse_UnableToLocateRecord_25 = "25";
       public readonly static string TrnxResponse_DuplicateRecord_26 = "26";
       public readonly static string TrnxResponse_FileUpdateEditError_27 = "27";
       public readonly static string TrnxResponse_FileUpdateFileLocked_28 = "28";
       public readonly static string TrnxResponse_FileUpdateFailed_29 = "29";
       public readonly static string TrnxResponse_FormatError_30 = "30";
       public readonly static string TrnxResponse_BankNotSupported_31 = "31";
       public readonly static string TrnxResponse_CompletedPartially_32 = "32";
       public readonly static string TrnxResponse_ExpiredCardPickUp_33 = "33";
       public readonly static string TrnxResponse_SuspectedFraudPickUp_34 = "34";
       public readonly static string TrnxResponse_ContactAcquirerPickUp_35 = "35";
       public readonly static string TrnxResponse_RestrictedCardPickUp_36 = "36";
       public readonly static string TrnxResponse_CallAcquirerSecurityPickup_37 = "37";
       public readonly static string TrnxResponse_PINTriesExceededPickup_38 = "38";
       public readonly static string TrnxResponse_NoCreditAccount_39 = "39";
       public readonly static string TrnxResponse_FunctionNotSupported_40 = "40";
       public readonly static string TrnxResponse_LostCard_41 = "41";
       public readonly static string TrnxResponse_NoUniversalAccount_42 = "42";
       public readonly static string TrnxResponse_StolenCard_43 = "43";
       public readonly static string TrnxResponse_NoInvestmentAccount_44 = "44";
       public readonly static string TrnxResponse_NotSufficientFunds_51 = "51";
       public readonly static string TrnxResponse_NoCheckAccount_52 = "52";
       public readonly static string TrnxResponse_NoSavingsAccount_53 = "53";
       public readonly static string TrnxResponse_ExpiredCard_54 = "54";
       public readonly static string TrnxResponse_IncorrectPIN_55 = "55";
       public readonly static string TrnxResponse_NoCardRecord_56 = "56";
       public readonly static string TrnxResponse_TransactionNotPermittedToCardHolder_57 = "57";
       public readonly static string TrnxResponse_TransactionNotPermittedOnTerminal_58 = "58";
       public readonly static string TrnxResponse_SuspectedFraud_59 = "59";
       public readonly static string TrnxResponse_ContactAcquirer_60 = "60";
       public readonly static string TrnxResponse_ExceedsWithdrawalLimit_61 = "61";
       public readonly static string TrnxResponse_RestrictedCard_62 = "62";
       public readonly static string TrnxResponse_SecurityViolation_63 = "63";
       public readonly static string TrnxResponse_OriginalAmountIncorrect_64 = "64";
       public readonly static string TrnxResponse_ExceedsWithdrawalFreqency_65 = "65";
       public readonly static string TrnxResponse_CallAcquirerSecurity_66 = "66";
       public readonly static string TrnxResponse_HardCapture_67 = "67";
       public readonly static string TrnxResponse_ResponseRecievedTooLate_68 = "68";
       public readonly static string TrnxResponse_PINTriesExceeded_75 = "75";
       public readonly static string TrnxResponse_InterveneBankApprovalRequired_77 = "77";
       public readonly static string TrnxResponse_InterveneBankApprovalRequiredforPartialAmount_78 = "78";
       public readonly static string TrnxResponse_Cut_offInProgress_90 = "90";
       public readonly static string TrnxResponse_IssuerOrSwitchInoperative_91 = "91";
       public readonly static string TrnxResponse_RoutingError_92 = "92";
       public readonly static string TrnxResponse_ViolationOfLaw_93 = "93";
       public readonly static string TrnxResponse_DuplicateTransaction_94 = "94";
       public readonly static string TrnxResponse_ReconcileError_95 = "95";
       public readonly static string TrnxResponse_SystemMalfunction_96 = "96";
       public readonly static string TrnxResponse_ExceedsCashLimit_98 = "98";

       #endregion

       #region // MTI Descriptor from FEP
       public readonly static string MTIDescriptorFEP_AuthorizationRequestResponse_110 = "110";
       public readonly static string MTIDescriptorFEP_FinancialRequestResponse_210 = "210";
       public readonly static string MTIDescriptorFEP_ReversalAdviceResponse_430 = "430";
       public readonly static string MTIDescriptorFEP_AuthorizationAdviceResponse_130 = "130";
       public readonly static string MTIDescriptorFEP_FinancialCompletionResponse_212 = "212";
       public readonly static string MTIDescriptorFEP_FinancialTransactionAdviceResponse_230 = "230";
       public readonly static string MTIDescriptorFEP_AdminResponse_610 = "610";
       #endregion

       #region // MTI Description from Source Node

       public readonly static string MTIDescriptorSource_AuthorizationRequest_100 = "100";
       public readonly static string MTIDescriptorSource_RepeatAuthorizationRequest_101 = "101";
       public readonly static string MTIDescriptorSource_FinancialRequest_200 = "200";
       public readonly static string MTIDescriptorSource_RepeatFinancialRequest_201 = "201";
       public readonly static string MTIDescriptorSource_ReversalAdvice_420 = "420";
       public readonly static string MTIDescriptorSource_RepeatReversalAdvice_421 = "421";
       #endregion
    }
}
