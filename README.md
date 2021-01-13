**<h1 dir="rtl">authin-net</h1>**
<p dir="rtl">Authin Client SDK for .NET</p>

<h2 dir="rtl">راهنمای نصب Authin.Api.Sdk در NET.</h2>

**<p dir="rtl">1. ابتدا کتابخانه <code>Authin.Api.Sdk.dll</code> که در <a href="https://github.com/authiniam/authin-net/releases">releases</a> وجود دارد را به رفرنس‌های پروژه خود اضافه کنید.</p>**

**<p dir="rtl">2. <code>NuGet pacakge</code>های زیر را بر روی پروژه مقصد نصب کنید:</p>**

```
Install-Package Newtonsoft.Json -Version 10.0.1
Install-Package Microsoft.IdentityModel.Tokens
Install-Package System.IdentityModel.Tokens.Jwt
```
<br/>
<h2 dir="rtl">راهنمای استفاده از Authin.Api.Sdk در NET.</h2>

<p dir="rtl">به منظور احراز هویت و احراز دسترسی کاربر، می‌بایست روال زیر به ترتیب انجام شود:</p>

**<p dir="rtl">1. هدایت کاربر به آدرس سامانه احراز هویت به روش زیر:</p>**


```csharp
var authorizationRequest = AuthorizationCodeRequest.GetBuilder()
    .SetBaseUrl("IAM_BASE_ADDRESS")                         	(1)
    .SetClientId("YOUR_CLIENT_ID")                          	(2)
    .SetRedirectUri("YOUR_REDIRECT_URI")                    	(3)
    .SetResponseType("code")                                	(4)
    .AddScopes(new List<string>() {"openid", "profile"})    	(5)
    .AddIdTokenClaim("username")                            	(6)
    .AddIdTokenClaim("lastname")                            	(7)
    .SetState("some_data")                                  	(8)
    .Build();

var authorizationResult = await authorizationRequest.Execute();	(9)
```
<ol dir="rtl">
	<li>آدرس سامانه احراز هویت مثال:  <code>https://demo.authin.ir</code></li>
	<li><code>client_id</code> سامانه شما در سامانه احراز هویت (این پارامتر را از ما دریافت می‌کنید)</li>
	<li><code>redirect_uri</code>ای که در تنظیمات سامانه شما در سامانه احراز هویت ثبت شده است. بعد از اتمام فرآیند احراز هویت، کاربر به همراه یک کد که اصطلاحا <code>authorization code</code> نام دارد به آدرس مذکور هدایت می‌شود. برای اطلاعات بیشتر به <a href="https://www.oauth.com/oauth2-servers/redirect-uris/">Redirect URIs</a> رجوع کنید. به طور مثال اگر <code>redirect_uri</code> شما برابر با <code>htt://my.domain.com/Account/ExternalLoginCallback</code> باشد، کاربر به آدرس <code>htt://my.domain.com/Account/ExternalLoginCallback?code=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</code> هدایت می‌شود. پارامتر <code>code</code> برای مرحله بعد لازم است.</li>
	<li>طبق پروتکل، نوع جوابی که خواهان دریافت آن هستیم را معین می‌کند که در این مرحله <code>"code"</code> می‌باشد.</li>
	<li>لیست دسترسی‌هایی که می‌خواهید بر روی توکن کاربر نوشته شود را تعیین می‌کنید.
		<ul>
			<li>خصیصه‌ی <code>openid</code> همواره باید درخواست داده شود.</li>
			<li>اگر غیر از مورد قبل، هیچ دسترسی دیگری را درخواست ندهید، تمامی دسترسی‌های موجود کاربر بر روی توکن نوشته می‌شود.</li>
			<li>اگر هر دسترسی خاصی را که مد نظر دارید درخواست دهید، فقط همان دسترسی‌ها بر روی توکن
    کاربر نوشته می‌شوند.</li>
		</ul>
	</li>
	<li>خصیصه‌های پروفایل کاربر را که می‌خواهید به درخواست اضافه کنید. این خصیصه بر روی <code>id_token</code> کاربر نوشته می‌شود.</li>
	<li>همانند مورد قبل، هر خصیصه‌ای را که مد نظر دارید، می‌توانید به درخواست اضافه کنید.</li>
    <li>پارامتر <code>state</code> این امکان را به شما می‌دهد تا وضعیت (state) قبلی سیستم خود را بازیابی کنید. این پارامتر بعد از بازهدایت کاربر به <code>redirect_uri</code> عینا همراه url ارسال می‌شود. برای مطالعه بیشتر به <a href="https://auth0.com/docs/protocols/oauth2/oauth-state">State Parameter</a> رجوع کنید.</li>
	<li>نتیجه، آدرسی است که به منظور احراز هویت کاربر، باید او را به آدرس مذکور هدایت کنید.</li>
</ol>
   
<br/>	 

**<p dir="rtl">2. در مرحله دوم، بعد از این که کاربر احراز هویت شد و به سامانه شما هدایت شد، نیاز است تا از سامانه احراز هویت، درخواست token برای کاربر مذکور بدهید. بدین منظور به روش زیر عمل کنید:</p>**

```csharp
var tokenRequest = TokenRequest.GetBuilder()
		.SetBaseUrl("IAM_BASE_ADDRESS")
		.SetClientId("YOUR_CLIENT_ID")          (1)
		.SetClientSecret("YOUR_CLIENT_SECRET")  (2)
		.SetRedirectUri("YOUR_REDIRECT_URI")    (3)
		.SetGrantType("authorization_code")     (4)
		.SetCode(code)                          (5)
		.Build();

var tokenResult = await tokenRequest.Execute();	(6)
```
<ol dir="rtl">
	<li><code>client_id</code> سامانه شما در سامانه احراز هویت (این پارامتر را از ما دریافت می‌کنید)</li>
	<li><code>client_secret</code> سامانه شما در سامانه احراز هویت (این پارامتر را از ما دریافت می‌کنید)</li>
	<li>مقدار <code>redirect_uri</code> که در مرحله قبل قرار دادید. توجه کنید باید این مقدار در هر دو مرحله یکی باشند.</li>
	<li>چارچوب <code>grant type</code>، <code>OAuth</code>های مختلفی را برای کاربردهای مختلف مشخص می‌کند. یکی از آن‌ها <code>authorization_code</code> است که برای درخواست توکن در ازای ارائه <code>Authorization Code</code> مورد استفاده قرار می‌گیرد. این مقدار را در این مرحله <code>"authorization_code"</code> قرار دهید. برای اطلاعات بیشتر به <a href="https://www.oauth.com/oauth2-servers/server-side-apps/authorization-code/">Authorization Code Grant</a> رجوع کنید.</li>
	<li><code>code</code>ای را که در زیر بخش ۳ از مرحله قبل دریافت کردید در این قسمت قرار دهید.</li>
	<li>پاسخ شامل پارامترهای <code>access_token</code>، <code>id_token</code> و <code>refresh_token</code> است.</li>
</ol>

<br/>	 

**<p dir="rtl">3. بعد از دریافت توکن لازم است تا اقدام به صحت سنجی توکن‌های دریافتی کنید. این کار دارای ۲ مرحله است:</p>**
<p dir="rtl"> 1. مرحله اول: دریافت کلید عمومی</p>

```csharp
var jwksRequest = JwksRequest.GetBuilder()
		.SetBaseUrl("IAM_BASE_ADDRESS")
		.Build();

var jwksResult = await jwksRequest.Execute();
```

<p dir="rtl"> 2. مرحله دوم: صحت سنجی و تجزیه توکن</p>

```csharp
var decodedJwt = TokenValidator.Validate(
			tokenResult.AccessToken,    (1)
			jwksResult,                 (2)
			"ISSUER",                   (3)
			"AUDIENCE"                  (4)
);
```
<ol dir="rtl">
	<li><code>access_token</code> دریافتی در مرحله ۲</li>
	<li>کلید عمومی دریافتی در مرحله ۱ از مراحل صحت سنجی</li>
	<li>صادر کننده توکن مثال <code>https://www.authin.ir</code></li>
	<li>شناسه سامانه‌ای که توکن برای آن صادر شده (<code>client_id</code>)</li>
</ol>
	
<ul dir="rtl">
	<li>جواب دریافتی شامل فقره‌های اطلاعاتی موجود در هر توکن می‌باشد. به طور مثال <code>scope</code>هایی که در مرحله ۱ درخواست داده شده‌اند در <code>access_token</code> هستند و یا <code>claim</code>هایی  که در مرحله ۱ در زیر بخش‌های شماره ۶ و ۷ اضافه شده‌اند را در تجزیه <code>id_token</code> دریافت خواهید کرد.
	</li>
</ul>

<blockquote dir="rtl"> توجه: به هیچ عنوان بدون صحت‌سنجی، توکن‌های دریافتی را استفاده نکنید. توکن‌ها به معنی اعتبارنامه دسترسی به سامانه شما هستند. </blockquote>

</br>

**<p dir="rtl">4. پس  از اتمام فرایند احراز هویت یکی از توکن‌های دریافتی <code>refresh_token</code> است که سامانه شما با ارائه این توکن به سامانه احراز هویت آتین می‌تواند بدون مداخله کاربر توکن جدیدی دریافت کند. مکانیزم درخواست <code>refresh_token</code> بدین صورت است که در صورتی که عمر توکن فعلی کاربر به اتمام رسیده و توکن منقضی شده باشد،  سامانه شما با ارائه <code>refresh_token</code> دریافتی، توکن جدیدی را به دست می‌آورد.بدین منظور به روش زیر عمل کنید:</p>**

```csharp
var refreshTokenRequest = RefreshTokenRequest.GetBuilder()
	.SetBaseUrl("IAM_BASE_ADDRESS")
	.SetClientId("YOUR_CLIENT_ID")
	.SetClientSecret("YOUR_CLIENT_SECRET")
	.SetAccessToken(tokenResult.AccessToken)	(1)
	.SetGrantType("refresh_token")			(2)
	.SetRefreshToken(tokenResult.RefreshToken)	(3)
	.Build();

var tokenResult = await tokenRequest.Execute();	(4)
```
<ol dir="rtl">
	<li><code>access_token</code> دریافتی در مرحله ۲</li>
	<li>این مقدار باید برابر با <code>refresh_token</code> باشد</li>
	<li><code>refresh_token</code> دریافتی در مرحله ۲</li>
	<li>جواب دریافتی شامل <code>access_token</code> و <code>id_token</code>های جدید همانند پاسخ مرحله 2 خواهد بود.</li>
</ol>

<blockquote dir="rtl">- لازم به ذکر است که به هنگام اجرای فرایند خروج از سامانه شما <code>refresh_token</code> نیز باید پاک شود.</blockquote>
<blockquote dir="rtl">- فراموش نکنید که توکن‌های دریافتی در این مرحله نیز باید صحت‌سنجی شوند.</blockquote>
</br>



**<p dir="rtl">5. برای دریافت اطلاعات کاربر که درخواست آن را در مرحله ۱ در <code>scope</code>ها داده‌اید، به روش زیر عمل کنید:</p>**

```csharp
var userInfoRequest = UserInfoRequest.GetBuilder()
		.SetBaseUrl("IAM_BASE_ADDRESS")
		.SetMethod(Method.Get)                      (1)
		.SetAccessToken(tokenResult.AccessToken)    (2)
		.Build();

var userInfoResult = await userInfoRequest.Execute();
```
<ol dir="rtl">
	<li>نوع درخواست که می‌تواند <code>GET</code> یا <code>POST</code> باشد.</li>
	<li><code>access_token</code> دریافتی در مرحله ۲.</li>
</ol>
<br/>


**<p dir="rtl">6. فرآیند خروج کاربر:</p>**

<p dir="rtl">فرآیند خروج کاربر به دو حالت زیر می‌تواند صورت پذیرد:</p>

<ul dir="rtl">
	<li>حالت اول: خروج از طریق سامانه احراز هویت مرکزی آتین<br/>
     در این حالت پس از تکمیل فرآیند خروج در سامانه احراز هویت، یک درخواست از نوع <code>POST</code> به آدرس <code>backchannelLogoutUri</code> که در تنظیمات سامانه شما مشخص شده است،  به صورت زیر ارسال می‌شود.  پس از دریافت درخواست و استخراج <code>logout_token</code>، توکن مربوطه را به روشی که در بخش 3 توضیح داده شده است، صحت‌سنجی کنید. نیاز است تا در جواب درخواست وارد شده یکی از سه <code>status code</code> زیر را به عنوان پاسخ برگردانید:
        <ul>
            <li><code>200</code> در صورت موفقیت آمیز بودن خروج کاربر در سامانه شما</li>
            <li><code>400</code> در صورتی که صحت‌سنجی توکن با موفقیت صورت نگیرد</li>
            <li><code>501</code> در صورتی که به هر دلیل دیگری قادر به تکمیل فرآیند خروج کاربر در سامانه خود نشوید</li>
        </ul>
	</li>
</ul>

```bash
curl --request POST \
     --url '<backchannel_logout_uri>' \
     --header 'content-type: application/x-www-form-urlencoded' \
     --data 'logout_token="eyJxxxxxxxxxxiJ9.eyJxxxxxxxxxxIn0.rNjxxxxxxxxxxb1E"'
```

<ul dir="rtl">
	<li>حالت دوم: خروج از طریق سامانه شما (<code>RP-Initiated Logout</code>)<br/> نیاز است به منظور خروج کاربر از سامانه احراز هویت و در نتیجه خروج از دیگر سامانه‌های مربوطه، پس از اینکه فرآیند خروج کاربر از سامانه شما انجام گرفت، کاربر را به آدرس صفحه خروج سامانه آتین هدایت کنید. این درخواست شامل پارامترهای زیر است:
	</li>
    <ul dir="rtl">
        <li><code>id_token_hint:</code> یکی از <code>ID Token</code>های دریافتی از سامانه آتین مرتبط با نشست فعلی کاربر</li>
        <li><code>post_logout_redirect_uri:</code> آدرس صفحه‌ای در سامانه شما، که پس از اتمام فرایند خروج، کاربر به آن صفحه هدایت می‌‌شود . لازم است این آدرس در سامانه آتین تعریف شده باشد.</li>
        <li><code>state:</code> در صورت استفاده از <code>post_logout_redirect_uri</code> این مقدار عینا در پاسخ به سامانه شما بازمی‌گردد.</li>
    </ul>
</ul>


<p dir="rtl">نمونه درخواست:</p>

```
https://<authin_idp_address>/logout?id_token_hint=YOUR_ID_TOKEN&post_logout_redirect_uri=YOUR_POST_LOGOUT_REDIRECT_URI&state=YOUR_STATE
```

<p dir="rtl">نمونه پاسخ:</p>

```
post_logout_redirect_uri?state=YOUR_STATE
```

<blockquote dir="rtl"> <strong>توجه:</strong> در صورتی که پارامتر  <code>id_token_hint</code> همراه درخواست ارسال نشود، فرآیند خروج کاربر به طور کامل <strong>اجرا می‌شود</strong> اما کاربر به آدرس <code>post_logout_redirect_uri</code> بازهدایت <strong>نخواهد شد</strong>.</blockquote>

