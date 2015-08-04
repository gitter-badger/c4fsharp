declare module c4fsharp {
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<any>;
        }
    }
    module Controls {
        interface EntryPoint {
            get_Body(): __ABBREV.__Client.IControlBody;
        }
    }
    module Client {
        var Start : {
            (input: string, k: {
                (x: string): void;
            }): void;
        };
        var Main : {
            (): __ABBREV.__Client.Element;
        };
    }
    interface Action {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Client = IntelliFactory.WebSharper.Html.Client;
}
