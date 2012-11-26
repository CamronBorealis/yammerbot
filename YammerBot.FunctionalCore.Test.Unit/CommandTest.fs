namespace YammerBot.FunctionalCore.Test.Unit

open YammerBot.FunctionalCore
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Given a common valid command`` ()=
    let command = Command("@@myCommand this is the arguments")

    [<Test>] member test.
     ``the CommandName should be the word following at-at`` ()=
        command.CommandName |> should equal "myCommand"

    [<Test>] member test.
     ``the Argument should be the text following the command`` ()=
        command.Argument |> should equal "this is the arguments"

[<TestFixture>]
type ``Given a mid-line command`` ()=
    let command = Command("Hey @yammerbot, you should @@tellmetofuckoff and die")

    [<Test>] member test.
     ``the CommandName should be the word following at-at`` ()=
        command.CommandName |> should equal "tellmetofuckoff"

    [<Test>] member test.
     ``the Argument should be the text following the command`` ()=
        command.Argument |> should equal "and die"

[<TestFixture>]
type ``Given a command without arguments`` ()=
    let command = Command("@@shoutout")

    [<Test>] member test.
     ``the CommandName should be the word following at-at`` ()=
        command.CommandName |> should equal "shoutout"

    [<Test>] member test.
     ``the Argument should be blank`` ()=
        command.Argument |> should equal ""

[<TestFixture>]
type ``Given an invalid command`` ()=
    let command = Command("Yammer bot is bugging me!")

    [<Test>] member test.
     ``the CommandName should be blank`` ()=
        command.CommandName |> should equal ""

    [<Test>] member test.
     ``the Argument should be blank`` ()=
        command.Argument |> should equal ""