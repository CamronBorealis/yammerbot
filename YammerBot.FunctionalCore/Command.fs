namespace YammerBot.FunctionalCore

open System.Text.RegularExpressions

type Command (command : string) =
    let regexResults = Regex(".*@@(?<command>\w*)(?<arguments>.*)").Match(command).Groups
    member this.CommandName
        with get() = 
            regexResults.Item("command").Value
    member this.Argument
        with get() =
            regexResults.Item("arguments").Value.Trim()