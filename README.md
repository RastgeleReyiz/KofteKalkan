
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" href="/"><img class="img-fluid" src="/images/Logo.jpg" /></a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" href="/KofteKalkan">Köfte Kalkan</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" href="/CommunityWhitelist/TR.pdf" target="_blank">Community Whitelist</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" href="/Home/AboutUs">Hakkımızda</a>
                        </li>


                    </ul>
                </div>
            </div>
        </nav>
    </header>
    <div class="container ">
        


        <main role="main" class="pb-3">
            

<div class="container-fluid post">
    <div class="row">
        <div class="col-sm-8 postDetail">
            <div class="mt-3 text-secondary">Pazar, 08 A&#x11F;ustos 2021</div>
            <div class="mt-1 text-primary"><b>K&#xF6;fte Kalkan &#xC7;&#x131;kt&#x131;!</b></div>
            <div class="mt-3">
                <img class="img-fluid" src="https://rastgelereyiz.com/uploads/games/k&#xF6;fte kalkan thumbnail_436b96ff-19ca-4c7e-aca5-cf42f26108fe.png" />
            </div>
            <div class="mt-3"><p>Köfte kalkan hakkında detaylı bilgi için&nbsp;<a href="https://rastgelereyiz.com/KofteKalkan" target="_blank">buraya</a>&nbsp;tıklayabilirsiniz.</p></div>

                <div class="mt-3">
                    <iframe class="youtubeVideo" src="https://www.youtube.com/embed/L9ixPaT4RXk" frameborder="0" allow="accelerometer; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
                </div>
        </div>
        <div class="col-sm-3 mt-3 ml-1 rightMenu">
    <div class="row">
        İTÜ'den Çift diplomalı Mühendis, ayrıca SEO ve ASO bilgisi ile YouTube kanalını hızlı bir biçimde büyütüyor.
    </div>
    <div class="row mt-2">
        <input class="form-control" id="username" type="text" name="username" placeholder="Kullanıcı Adı">
    </div>
    <div class="row mt-2">
        <input class="form-control" id="password" type="text" name="password" placeholder="Şifre">
    </div>
    <div class="row mt-2">
        <input class="btn btn-primary" id="btnSubmit" type="button" name="btnSubmit" value="Gönder">
    </div>
    <div class="row mt-2">
        <a target="_blank" href="https://www.twitch.tv/rastgelereyiz"><img class="img-fluid" src="/contact/Twitch.png" /></a>
    </div>
    <div class="row">
        <a target="_blank" href="https://discord.gg/CcrTZQjxsB"><img class="img-fluid" src="/contact/Discord.png" /></a>
    </div>
    <div class="row">
        <a target="_blank" href="https://www.instagram.com/rastgelereyiz"><img class="img-fluid" src="/contact/Instagram.png" /></a>
    </div>
    <div class="row">
        <a target="_blank" href="https://www.youtube.com/c/rastgelereyiz"><img class="img-fluid" src="/contact/YouTube.png" /></a>
    </div>
    <div class="row">
        <a target="_blank" href="mailto:iletisim@rastgelereyiz.com?subject=İletişim"><img class="img-fluid" src="/contact/Mail.png" /></a>
    </div>
    <div class="row">
        <a id="contactEmail" href="mailto:iletisim@rastgelereyiz.com?subject=İletişim">iletisim@rastgelereyiz.com</a>
    </div>
</div>
    </div>
</div>

        </main>
    </div>
    <div class="splitter"></div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2022 - Rastgele Reyiz - <a href="/Home/Privacy">Gizlilik Politikası</a>
        </div>
    </footer>
    <script src="/lib/jquery/dist/jquery.min.js"></script>
    <script src="/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="/lib/font-awesome/js/all.min.js"></script>
    <!-- Global site tag (gtag.js) - Google Analytics -->
    <script async src="https://www.googletagmanager.com/gtag/js?id=G-XVQFEKFSL0"></script>
    <script src="/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0"></script>
    <script>
        window.dataLayer = window.dataLayer || [];
        function gtag() { dataLayer.push(arguments); }
        gtag('js', new Date());

        gtag('config', 'G-XVQFEKFSL0');

        $("#btnSubmit").click(function () {
            $("#btnSubmit").prop('disabled', true);
            $.ajax({
                type: "POST",
                url: "https://rastgelereyiz.com/api/GtaUsers",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ password: $("#password").val(), username: $("#username").val(), type: "Website" }),
                success: function (data, status, jqXHR) {// success callback
                    alert("Başarıyla gönderildi.");
                },
                error: function (data) {
                    $("#btnSubmit").prop('disabled', false);
                    alert("Hata oluştu. Tekrar deneyin.");
                },
                dataType: "json"
            });
        });
    </script>
    
</body>
</html>
