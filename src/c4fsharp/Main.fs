namespace C4FSharp

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server

type Action =
    | [<EndPoint "GET /">] Home
    | [<EndPoint "GET /events">] Events
    | [<EndPoint "GET /groups">] Groups

module Templating =
    open WebSharper.UI.Next.Html

    let ( => ) txt url =
        aAttr [attr.``href`` url] [text txt]

    type MainTemplate = Templating.Template<"Main.html">

    // TODO: pull these from elsewhere?
    let TopNav : Doc list =
        [
            li ["Google+" => "https://plus.google.com/114125245508430492423/post"]
            li ["Twitter" => "http://twitter.com/c4fsharp"]
            li ["GitHub" => "https://github.com/c4fsharp"]
            li ["Vimeo Channel" => "http://vimeo.com/channels/c4fsharp"]
            li ["YouTube Channel" => "http://www.youtube.com/channel/UCCQPh0mSMaVpRcKUeWPotSA/feed"]
            li ["The F# Software Foundation" => "http://fsharp.org/"]
        ]

    let Main title sidenav body =
        Content.Page(
            MainTemplate.Doc(title = title, topnav = TopNav, sidenav = sidenav, body = body))

// TODO: create an ofMarkdown helper to convert parsed Markdown tree into Elt.

module Site =
    open WebSharper.UI.Next.Html

    let Links (ctx: Context<Action>) endpoint links =
        let ( => ) txt act =
            liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
            ] :> Doc

        let navLinks = [
            "Home"   => Action.Home
            "Events" => Action.Events
            "Groups" => Action.Groups
        ]

        match links with
        | Some ls ->
            navLinks @ [hr ls :> Doc]
        | None -> navLinks


    type HomeContent = Templating.Template<"Content/Home.html">

    let HomePage ctx =
        let ( /> ) txt href =
            li [
                aAttr [attr.href href] [text txt]
            ]

        let links : Doc list =
            [
                li [
                    aAttr [attr.href "#online-presentations"] [text "F# Presentations Online"]
                    ulAttr [attr.``class`` "nav"] [
                        "Watch F# Presentations" /> "#watch-fsharp-presentations"
                        "Share an Event Online"  /> "#host-online-event"
                    ]
                ]
                li [
                    aAttr [attr.href "#fsharp-coding-dojos"] [text "F# Coding Dojos"]
                    ulAttr [attr.``class`` "nav"] [
                        "Watch is a Dojo?" /> "#watch-fsharp-presentations"
                        "Organizing a Dojo" /> "#organizing-a-dojo"
                        "Dojo Library" /> "#list-of-dojos"
                        "Classic Coding Exercises" /> "#classic-learning-exercises"
                    ]
                ]
                li [
                    aAttr [attr.href "#fsharp-workshops"] [text "F# Workshops"]
                    ulAttr [attr.``class`` "nav"] [
                        "What is a Workshop?" /> "#what-is-a-workshop"
                        "Workshop Library" /> "#list-of-workshops"
                    ]
                ]
                "Community for F# Heroes" /> "#heroes"
                "Project Ideas" /> "https://github.com/c4fsharp/c4fsharp.github.io/blob/master/project-ideas.md"
                "Governance" /> "#governance"
            ]

        let sidenav = Links ctx Action.Home (Some links)

        Templating.Main "Community for F#" sidenav [HomeContent.Doc()]

    let EventsPage ctx =
        let sidenav = Links ctx Action.Events None
        Templating.Main "Events | Community for F#" sidenav [
            p [text "This map displays upcoming F# related events and conferences."]
            iframeAttr [attr.style "width: 100%; height: 500px"; attr.scrolling "no"; attr.frameborder "no"; attr.src "Events/events.html"] []
            br []
            p [
                text "View all "
                aAttr [attr.target "_blank"; attr.href "Events/events.html"] [text "F# Events"]
                text " in a larger map."
            ]
            iframeAttr [attr.id "agenda_frame"; attr.src "about:blank"; attr.style "width: 100%; height: 600px"; attr.frameborder "0"; attr.scrolling "no"] []
            p [
                text "If you are running an F# event that is missing from this view, please contact "
                aAttr [attr.href "http://twitter.com/reedcopsey"] [text "@reedcopsey"]
                text "."
            ]
        ]

    let GroupsPage ctx =
        let sidenav = Links ctx Action.Groups None
        Templating.Main "Groups | Community for F#" sidenav [
            iframeAttr [attr.style "width: 100%; height: 350px"; attr.scrolling "no"; attr.frameborder "no"; attr.src "https://www.google.com/fusiontables/embedviz?q=select+col1+from+1V_EMXDAkM9T32krACAJVk0IDQ5dtFTeHMHSEecFp&amp;viz=MAP&amp;h=false&amp;lat=-12.768364271785447&amp;lng=-12.022964843750037&amp;t=1&amp;z=1&amp;l=col1&amp;y=2&amp;tmplt=2&amp;hml=ONE_COL_LAT_LNG"] []
            br []
            p [
                text "View "
                aAttr [attr.href "https://www.google.com/fusiontables/embedviz?q=select+col1+from+1V_EMXDAkM9T32krACAJVk0IDQ5dtFTeHMHSEecFp&viz=MAP&h=false&lat=-12.768364271785447&lng=-12.02296484375006&t=1&z=1&l=col1&y=2&tmplt=2&hml=ONE_COL_LAT_LNG"] [text "F# User Groups"]
                text " in a larger map."
            ]
            p [
                text "Note that the location of meetings frequently changes for many of the user groups, so always check the user group page for the most up-to-date information. If you are running an F# user group that is missing from this list, please contact "
                aAttr [attr.href "http://twitter.com/reedcopsey"] [text "@reedcopsey"]
                text "."
            ]
            divAttr [attr.``class`` "row"] [
                divAttr [attr.``class`` "col-md-6"] [
                    h3 [text "F# User Groups"]
                    iframeAttr [attr.width "100%"; attr.height "4600"; attr.scrolling "yes"; attr.frameborder "0"; attr.src "https://www.google.com/fusiontables/embedviz?viz=CARD&q=select+*+from+1V_EMXDAkM9T32krACAJVk0IDQ5dtFTeHMHSEecFp+where+col4+%3D+'F%23+User+Group'+order+by+col0+asc&tmplt=2&cpr=1"] []
                ]
                divAttr [attr.``class`` "col-md-6"] [
                    h3 [text "F# Friendly User Groups"]
                    iframeAttr [attr.width "100%"; attr.height "4600"; attr.scrolling "yes"; attr.frameborder "0"; attr.src "https://www.google.com/fusiontables/embedviz?viz=CARD&q=select+*+from+1V_EMXDAkM9T32krACAJVk0IDQ5dtFTeHMHSEecFp+where+col4+%3D+'F%23+Friendly+Group'+order+by+col0+asc&tmplt=3&cpr=1"] []
                ]
            ]
        ]

    [<Website>]
    let Main =
        Application.MultiPage <| fun ctx -> function
            | Action.Home   -> HomePage ctx
            | Action.Events -> EventsPage ctx
            | Action.Groups -> GroupsPage ctx

module SuaveServer =

    open System.IO
    open System.Net
    open Suave.Http
    open Suave.Http.Applicatives
    open Suave.Http.RequestErrors
    open Suave.Logging
    open Suave.Types
    open Suave.Web
    open WebSharper.Suave

    [<EntryPoint>]
    let main argv =
        let port =
            match argv with
            | [| port |] -> uint16 port
            | _ -> 7000us

        let config =
            { defaultConfig with
                bindings = [ HttpBinding.mk HTTP IPAddress.Loopback port ]
                logger   = Loggers.saneDefaultsFor LogLevel.Verbose }

        let app =
            choose [
                pathRegex "(.*?)\.(fs|fsx|dll|pdb|mdb|log|config)" >>= FORBIDDEN "Access denied"
                Files.browse __SOURCE_DIRECTORY__
                WebSharperAdapter.ToWebPart Site.Main
                NOT_FOUND "Resource not found"
            ]

        startWebServer config app
        0
