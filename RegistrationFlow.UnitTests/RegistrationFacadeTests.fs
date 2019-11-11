module Ploeh.Samples.RegistrationFacadeTests

open Xunit
open Swensen.Unquote
open Ploeh.Samples.Tests.Fakes
open Ploeh.Samples.Registration

let createFixture () =
    let twoFA = Fake2FA ()
    let db = FakeRegistrationDB ()
    let sut =
        completeRegistrationWorkflow
            twoFA.CreateProof
            twoFA.VerifyProof
            db.CompleteRegistration
    sut, twoFA, db

[<Theory>]
[<InlineData 123>]
[<InlineData 432>]
let ``Missing proof ID`` mobile = async {
    let sut, twoFA, db = createFixture ()
    let r = { Mobile = Mobile mobile }

    let! actual = sut None r

    let! expectedProofId = twoFA.CreateProof r.Mobile
    let expected = ProofRequired expectedProofId
    expected =! actual
    test <@ Seq.isEmpty db @> }