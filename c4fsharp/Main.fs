namespace c4fsharp

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

    let TopNav : Doc list =
        [
            li ["Google+" => "https://plus.google.com/114125245508430492423/post"]
            li ["Twitter" => "http://twitter.com/c4fsharp"]
            li ["GitHub" => "https://github.com/c4fsharp"]
            li ["Vimeo Channel" => "http://vimeo.com/channels/c4fsharp"]
            li ["YouTube Channel" => "http://www.youtube.com/channel/UCCQPh0mSMaVpRcKUeWPotSA/feed"]
            li ["The F# Software Foundation" => "http://fsharp.org/"]
        ]

    let Main ctx action title body =
        Content.Doc(
            MainTemplate.Doc(title = title, topnav = TopNav, body = body))

module Site =
    open WebSharper.UI.Next.Html

    let Links (ctx: Context<Action>) endpoint : Doc =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]

        divAttr [attr.``class`` "col-md-3"] [
            divAttr [attr.``class`` "c4-sidebar hidden-print affix-top"] (* role "complementary" *) [
                ulAttr [attr.``class`` "nav c4-sidenav"] [
                    "Home"   => Action.Home
                    "Events" => Action.Events
                    "Groups" => Action.Groups
                ]
            ]
        ] :> _

    let HomePage ctx =
        Templating.Main ctx Action.Home "Community for F#" [
            Links ctx Action.Home
            divAttr [attr.``class`` "col-md-9"] [
                articleAttr [attr.id "online-presentations"] [
                    header [
                        h1 [text "Online F# Presentations"]
                    ]
                ]

                sectionAttr [attr.id "watch-fsharp-presentations"] [
                    header [
                        h2 [text "Watch F# Presentations"]
                    ]
                    p [
                        text "Community for F# has been organizing and recording F# talks since way before it was cool! Our "
                        aAttr [attr.``href`` "http://vimeo.com/channels/c4fsharp"] [text "Vimeo"]
                        text " and "
                        aAttr [attr.``href`` "http://www.youtube.com/channel/UCCQPh0mSMaVpRcKUeWPotSA/feed"] [text "YouTube"]
                        text " channels have videos of presentations covering everything from Agents to Data Science, Domain-Specific Languages or writing a F# compiler to JavaScript. And much, much more! So check it out, and let us know what you like and don't like, and what (or who) you want to see recorded."
                    ]
                ]

                sectionAttr [attr.id "host-online-event"] [
                    header [
                        h2 [text "Broadcast a Presentation Online"]
                    ]
                    p [text "Community for F# aims to give access to great learning resources to everyone in the Community, wherever they may be. If you run a Meetup group and have a great speaker coming in town, we will be happy to help you make that available online. And if you feel like giving a presentation online, let us know, we would love to have you on Community for F#!"]
                ]
            ]
        ]

    let EventsPage ctx =
        Templating.Main ctx Action.Events "Events | Community for F#" [
            Links ctx Action.Events
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
        Templating.Main ctx Action.Groups "Groups | Community for F#" [
            Links ctx Action.Groups
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

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin

    [<EntryPoint>]
    let Main = function
        | [| rootDirectory; url |] ->
            use server = WebApp.Start(url, fun appB ->
                appB.UseStaticFiles(
                        StaticFileOptions(
                            FileSystem = PhysicalFileSystem(rootDirectory)))
                    .UseSitelet(rootDirectory, Site.Main)
                |> ignore)
            stdout.WriteLine("Serving {0}", url)
            stdin.ReadLine() |> ignore
            0
        | _ ->
            eprintfn "Usage: c4fsharp ROOT_DIRECTORY URL"
            1