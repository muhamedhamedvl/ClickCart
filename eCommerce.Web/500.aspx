<% Response.StatusCode = 500 %>

<% Response.StatusCode = 404 %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <title>500 - Internal Server Error</title>
    <meta name="description" content="500 - Internal Server Error">
    <meta property="og:title" content="500 - Internal Server Error" />
    <meta property="og:description" content="500 - Internal Server Error" />

    <!--jQuery-->
    <script src="/Content/lib/jquery-3.4.1/jquery.min.js"></script>

    <!--Bootstrap-->
    <link href="/Content/lib/bootstrap-4.4.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="/Content/lib/bootstrap-4.4.1/js/bootstrap.min.js"></script>

    <!--Fontawesome fonts-->
    <link href="/Content/lib/fontawesome-free-5.13.0-web/css/all.min.css" rel="stylesheet" />

    <style>
        .masthead {
            height: 100vh;
            min-height: 500px;
            background-image: url('/Content/images/site/pages/error-page.jpg');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }
    </style>
</head>
<body class="text-center">
    <header class="masthead">
        <div class="container h-100">
            <div class="row h-100 align-items-center">
                <div class="col-12 text-center">
                    <h4 class="display-1">
                        <i class="fas fa-exclamation-triangle"></i>
                    </h4>
                    <h1 class="display-1">
                        500
                    </h1>
                    <p class="lead">
                        Internal Server Error. Please try again later.
                    </p>
                    <a href="/">Go to Home</a>
                </div>
            </div>
        </div>
    </header>
</body>
</html>