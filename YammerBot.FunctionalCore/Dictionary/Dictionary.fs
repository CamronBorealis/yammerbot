namespace YammerBot.FunctionalCore

open System.Text.RegularExpressions                                                                                                     

type IDictionary = 
    abstract member GetDefinition : string -> string
    abstract member IsNoun : string -> bool

                                                                                                                                        
type Dictionary (lookupService : IDictionaryService) = 
    interface IDictionary with
        member this.IsNoun word =                                                                                                                       
            let regex = Regex("^.*, n\.")                                                                                                     
            word                                                                                                                                
            |> lookupService.GetDefinitions                                                                                                                  
            |> Seq.exists (fun w -> regex.IsMatch w)                                                                                            
                                                                                                                                            
        member this.GetDefinition word =                                                                                                                
            word                                                                                                                                
            |> lookupService.GetDefinitions                                                                                                                   
            |> Seq.head 