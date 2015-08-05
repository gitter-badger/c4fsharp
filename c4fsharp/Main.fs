namespace c4fsharp

open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets

type Action =
    | [<EndPoint "GET /">] Home
    | [<EndPoint "GET /about">] About

module Controls =

    [<Sealed>]
    type EntryPoint() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            Client.Main() :> _

module Common = 

    let ( => ) text url =
        A [HRef url] -< [Text text]

module Skin =
    open Common

    let TopNav =
        UL [Class "nav navbar-nav"] -< [
            LI ["Google+" => "https://plus.google.com/114125245508430492423/post"]
            LI ["Twitter" => "http://twitter.com/c4fsharp"]
            LI ["GitHub" => "https://github.com/c4fsharp"]
            LI ["Vimeo Channel" => "http://vimeo.com/channels/c4fsharp"]
            LI ["YouTube Channel" => "http://www.youtube.com/channel/UCCQPh0mSMaVpRcKUeWPotSA/feed"]
            LI ["The F# Software Foundation" => "http://fsharp.org/"]
        ]

    type Page =
        {
            Title : string
            TopNav : Element
            SideNav : Element
            Body : Element list
        }
        static member Default =
            {
                Title = "Community for F#"
                TopNav = TopNav
                SideNav = []
                Body = []
            }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("topnav", fun x -> x.TopNav)
            .With("sidenav", fun x -> x.SideNav)
            .With("body", fun x -> x.Body)

    let WithTemplate title sidenav body =
        Content.WithTemplate MainTemplate
            { Page.Default with
                Title = title
                SideNav = sidenav
                Body = body
            }

module Site =
    open Common

    let Links (ctx: Context<Action>) =
        UL [
            LI ["Home" => ctx.Link Home]
            LI ["About" => ctx.Link About]
        ]

    let HomePage ctx =
        Skin.WithTemplate "Community for F#"
            (Links ctx)
            [
                Div [Text "HOME"]
                Div [new Controls.EntryPoint()]
            ]

    let AboutPage ctx =
        Skin.WithTemplate "About"
            (Links ctx)
            [
                Div [Text "ABOUT"]
            ]

    let MainSitelet =
        Application.MultiPage <| fun ctx -> function
            | Action.Home -> HomePage ctx
            | Action.About -> AboutPage ctx

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
                    .UseSitelet(rootDirectory, Site.MainSitelet)
                |> ignore)
            stdout.WriteLine("Serving {0}", url)
            stdin.ReadLine() |> ignore
            0
        | _ ->
            eprintfn "Usage: c4fsharp ROOT_DIRECTORY URL"
            1
