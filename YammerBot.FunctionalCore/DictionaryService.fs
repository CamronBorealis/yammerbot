namespace YammerBot.FunctionalCore

open System.Net                                                                                                                         
open System.Xml                                                                                                                         

type IDictionaryService = 
    abstract member GetDefinitions : string -> seq<string>

type DictionaryService() = 

    let ExtractDefinitions resultXml =                                                                                              
        let doc = new XmlDocument()                                                                                                 
        doc.LoadXml resultXml;                                                                                                      
        let nsmgr = XmlNamespaceManager(doc.NameTable)                                                                              
        nsmgr.AddNamespace("aon", "http://services.aonaware.com/webservices/");                                                     
        doc.SelectNodes("/aon:WordDefinition/aon:Definitions/aon:Definition/aon:WordDefinition/text()", nsmgr)                      
          |> Seq.cast<XmlNode>                                                                                                      
          |> Seq.map (fun node -> node.Value)                                                                                       
    interface IDictionaryService with
        member this.GetDefinitions (word : string) =                                                                                           
            try                                                                                                                                 
                let req = WebRequest.Create("http://services.aonaware.com//DictService/DictService.asmx/DefineInDict?dictId=gcide&word="+word)  
                use rsp = req.GetResponse()                                                                                                     
                use stream = rsp.GetResponseStream()                                                                                            
                use reader = new System.IO.StreamReader(stream)                                                                                 
                ExtractDefinitions (reader.ReadToEnd())                                                                                         
            with                                                                                                                                
                | _ -> Seq.empty                                                                                                                