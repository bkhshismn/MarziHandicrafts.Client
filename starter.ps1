# =========================
# Marzi Handicrafts Starter Kit
# .NET 9 + MAUI + Blazor Hybrid + MudBlazor
# =========================

$root = 'D:\Projects\MarziHandicrafts.Client'

Write-Host 'Creating folders...' -ForegroundColor Green

$folders = @(
    'Marzi.Client.Shared\Components\AppBar',
    'Marzi.Client.Shared\Components\Cards',
    'Marzi.Client.Shared\Components\Common',
    'Marzi.Client.Shared\Components\Navigation',
    'Marzi.Client.Shared\Layouts',
    'Marzi.Client.Shared\Pages\Auth',
    'Marzi.Client.Shared\Pages\Home',
    'Marzi.Client.Shared\Pages\Courses',
    'Marzi.Client.Shared\Pages\Portfolio',
    'Marzi.Client.Shared\Pages\Orders',
    'Marzi.Client.Shared\Pages\Profile',
    'Marzi.Client.Shared\Services',
    'Marzi.Client.Shared\Theme',
    'Marzi.Client.Shared\Assets\Images',
    'Marzi.Client.Shared\Assets\Icons',
    'Marzi.Client.Shared\Assets\Fonts'
)

foreach ($f in $folders) {
    New-Item -ItemType Directory -Force -Path (Join-Path $root $f) | Out-Null
}

Write-Host 'Creating Theme...' -ForegroundColor Green

@'
using MudBlazor;

namespace Marzi.Client.Shared.Theme;

public static class MarziTheme
{
    public static MudTheme Default = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#8B5E3C",
            Secondary = "#D2B48C",
            Background = "#FAF8F4",
            Surface = "#FFFFFF",
            AppbarBackground = "#8B5E3C",
            AppbarText = "#FFFFFF",
            DrawerBackground = "#FFFFFF",
            DrawerText = "#5A3E2B",
            TextPrimary = "#3B2A1F",
            TextSecondary = "#7A6657"
        }
    };
}
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Theme\MarziTheme.cs')

Write-Host 'Creating MainLayout...' -ForegroundColor Green

@'
@inherits LayoutComponentBase
@using MudBlazor
@using Marzi.Client.Shared.Components.Navigation

<MudThemeProvider Theme="@Marzi.Client.Shared.Theme.MarziTheme.Default" />
<MudDialogProvider />
<MudSnackbarProvider />

<MudLayout RightToLeft="true">
    <MudAppBar Elevation="1" Color="Color.Primary">
        <MudText Typo="Typo.h6" Class="mx-2">Marzi Handicrafts</MudText>
        <MudSpacer />
        <MudIconButton Icon="@Icons.Material.Filled.Notifications" Color="Color.Inherit" />
    </MudAppBar>

    <MudMainContent>
        <div class="pa-4 pb-10">
            @Body
        </div>
    </MudMainContent>

    <BottomNavigation />
</MudLayout>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Layouts\MainLayout.razor')

Write-Host 'Creating BottomNavigation...' -ForegroundColor Green

@'
@using MudBlazor

<MudPaper Elevation="6" Class="bottom-nav">
    <MudGrid Class="ma-0 pa-0" Justify="Justify.SpaceAround" AlignItems="AlignItems.Center">
        <MudItem>
            <MudNavLink Href="/" Match="NavLinkMatch.All" Icon="@Icons.Material.Filled.Home" Class="nav-link">
                خانه
            </MudNavLink>
        </MudItem>

        <MudItem>
            <MudNavLink Href="/courses" Icon="@Icons.Material.Filled.School" Class="nav-link">
                آموزش
            </MudNavLink>
        </MudItem>

        <MudItem>
            <MudNavLink Href="/portfolio" Icon="@Icons.Material.Filled.Palette" Class="nav-link">
                نمونه کار
            </MudNavLink>
        </MudItem>

        <MudItem>
            <MudNavLink Href="/orders" Icon="@Icons.Material.Filled.ShoppingBag" Class="nav-link">
                سفارش
            </MudNavLink>
        </MudItem>

        <MudItem>
            <MudNavLink Href="/profile" Icon="@Icons.Material.Filled.Person" Class="nav-link">
                پروفایل
            </MudNavLink>
        </MudItem>
    </MudGrid>
</MudPaper>

<style>
    .bottom-nav {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        z-index: 1200;
        border-top-left-radius: 18px;
        border-top-right-radius: 18px;
        padding: 8px 0;
    }

    .nav-link {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        gap: 2px;
        min-width: 64px;
        text-decoration: none;
        color: #6B5A4A;
        font-size: 12px;
    }

    .nav-link.active {
        color: #8B5E3C;
        font-weight: 600;
    }

    .nav-link .mud-icon-root {
        font-size: 26px;
    }
</style>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Components\Navigation\BottomNavigation.razor')

Write-Host 'Creating HeroBanner...' -ForegroundColor Green

@'
@using MudBlazor

<MudPaper Class="hero-banner pa-6 mb-4" Elevation="0">
    <MudText Typo="Typo.h4" Color="Color.Inherit" Class="mb-2">
        خلق هنر با دستان شما
    </MudText>

    <MudText Typo="Typo.body1" Color="Color.Inherit" Class="mb-4">
        آموزش نقاشی، مجسمه‌سازی و ثبت سفارش آثار هنری اختصاصی
    </MudText>

    <MudButton Variant="Variant.Filled" Color="Color.Secondary" Href="/courses" StartIcon="@Icons.Material.Filled.PlayArrow">
        شروع یادگیری
    </MudButton>
</MudPaper>

<style>
    .hero-banner {
        background: linear-gradient(135deg, #8B5E3C 0%, #B58A62 100%);
        color: white;
        border-radius: 28px;
    }
</style>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Components\Common\HeroBanner.razor')

Write-Host 'Creating SectionTitle...' -ForegroundColor Green

@'
@using MudBlazor

<Row Justify="Justify.SpaceBetween" Align="Align.Center" Class="mb-3">
    <MudText Typo="Typo.h6" Class="fw-bold">@Title</MudText>

    @if (!string.IsNullOrWhiteSpace(ActionText))
    {
        <MudLink Href="@ActionLink" Color="Color.Primary" Underline="Underline.None">
            @ActionText
        </MudLink>
    }
</Row>

@code {
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string? ActionText { get; set; }
    [Parameter] public string ActionLink { get; set; } = "#";
}
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Components\Common\SectionTitle.razor')

Write-Host 'Creating CourseCard...' -ForegroundColor Green

@'
@using MudBlazor

<MudCard Class="course-card" Elevation="2">
    <MudCardMedia Image="https://images.unsplash.com/photo-1513364776144-60967b0f800f?auto=format&fit=crop&w=800&q=80" Height="170" />

    <MudCardContent>
        <MudText Typo="Typo.subtitle1" Class="fw-bold mb-1">آموزش نقاشی مدرن</MudText>
        <MudText Typo="Typo.body2" Color="Color.Secondary" Class="mb-2">۱۲ جلسه • مقدماتی تا پیشرفته</MudText>

        <Row Justify="Justify.SpaceBetween" Align="Align.Center">
            <MudChip Color="Color.Primary" Variant="Variant.Outlined" Size="Size.Small">
                آنلاین
            </MudChip>

            <MudText Typo="Typo.subtitle2" Color="Color.Primary" Class="fw-bold">
                ۴۹۰ هزار تومان
            </MudText>
        </Row>
    </MudCardContent>

    <MudCardActions>
        <MudButton FullWidth="true" Variant="Variant.Filled" Color="Color.Primary" Href="/courses/1">
            مشاهده دوره
        </MudButton>
    </MudCardActions>
</MudCard>

<style>
    .course-card {
        border-radius: 20px;
        overflow: hidden;
    }
</style>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Components\Cards\CourseCard.razor')

Write-Host 'Creating PortfolioCard...' -ForegroundColor Green

@'
@using MudBlazor

<MudCard Class="portfolio-card" Elevation="1">
    <MudCardMedia Image="https://images.unsplash.com/photo-1515378791036-0648a3ef77b2?auto=format&fit=crop&w=800&q=80" Height="200" />

    <MudCardContent>
        <MudText Typo="Typo.subtitle1" Class="fw-bold">مجسمه سفارشی دست‌ساز</MudText>
        <MudText Typo="Typo.body2" Color="Color.Secondary">پلی‌استر • رنگ اکریلیک • ۱۴۰۵</MudText>
    </MudCardContent>
</MudCard>

<style>
    .portfolio-card {
        border-radius: 22px;
        overflow: hidden;
    }
</style>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Components\Cards\PortfolioCard.razor')

Write-Host 'Creating Home Page...' -ForegroundColor Green

@'
@page "/"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor
@using Marzi.Client.Shared.Components.Common
@using Marzi.Client.Shared.Components.Cards

<HeroBanner />

<MudText Typo="Typo.h5" Class="mb-2">سلام 🌸</MudText>
<MudText Typo="Typo.body1" Color="Color.Secondary" Class="mb-4">
    امروز آماده‌ای یک اثر هنری جدید خلق کنیم؟
</MudText>

<SectionTitle Title="دوره‌های آموزشی" ActionText="همه" ActionLink="/courses" />

<MudGrid Class="mb-4">
    <MudItem xs="12" sm="6" md="4" lg="3">
        <CourseCard />
    </MudItem>

    <MudItem xs="12" sm="6" md="4" lg="3">
        <CourseCard />
    </MudItem>
</MudGrid>

<SectionTitle Title="نمونه کارها" ActionText="همه" ActionLink="/portfolio" />

<MudGrid Class="mb-4">
    <MudItem xs="12" sm="6" md="4" lg="3">
        <PortfolioCard />
    </MudItem>

    <MudItem xs="12" sm="6" md="4" lg="3">
        <PortfolioCard />
    </MudItem>
</MudGrid>

<MudPaper Class="pa-5" Elevation="1" Style="border-radius:20px;">
    <Row Align="Align.Center" ColumnGap="3">
        <MudAvatar Icon="@Icons.Material.Filled.ShoppingBag" Color="Color.Primary" Size="Size.Large" />

        <Box Flex="1" Gap="1" Direction="Column">
            <MudText Typo="Typo.h6" Class="fw-bold">سفارش ساخت اثر هنری</MudText>
            <MudText Typo="Typo.body2" Color="Color.Secondary">
                عکس ایده‌ات را ارسال کن تا برایت مجسمه یا تابلو اختصاصی ساخته شود.
            </MudText>
        </Box>
    </Row>

    <MudButton Variant="Variant.Filled" Color="Color.Primary" Href="/orders" FullWidth="true" Class="mt-4">
        ثبت سفارش
    </MudButton>
</MudPaper>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Home\Home.razor')

Write-Host 'Creating Login Page...' -ForegroundColor Green

@'
@page "/login"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor

<Box Class="d-flex justify-center align-center" Style="min-height:70vh;">
    <MudPaper Class="pa-6" Width="100%" MaxWidth="420px" Elevation="3" Style="border-radius:24px;">
        <Box Class="d-flex justify-center mb-3">
            <MudAvatar Size="Size.Large" Color="Color.Primary" Icon="@Icons.Material.Filled.Palette" />
        </Box>

        <MudText Typo="Typo.h5" Align="Align.Center" Class="fw-bold mb-1">
            ورود به Marzi Handicrafts
        </MudText>

        <MudText Typo="Typo.body2" Align="Align.Center" Color="Color.Secondary" Class="mb-4">
            با شماره موبایل و رمز عبور وارد شوید
        </MudText>

        <MudTextField Label="شماره موبایل" Variant="Variant.Outlined" FullWidth="true" Class="mb-3" />

        <MudTextField Label="رمز عبور" Variant="Variant.Outlined" InputType="InputType.Password" FullWidth="true" Class="mb-2" />

        <MudCheckBox Label="مرا به خاطر بسپار" Color="Color.Primary" Class="mb-3" />

        <MudButton Variant="Variant.Filled" Color="Color.Primary" FullWidth="true" Size="Size.Large">
            ورود
        </MudButton>

        <Box Class="d-flex justify-space-between mt-3" Style="font-size:14px;" Wrap="Wrap.Wrap" RowGap="1">
            <MudLink Href="/register" Color="Color.Primary" Underline="Underline.None">ثبت نام</MudLink>
            <MudLink Href="/forgot-password" Color="Color.Primary" Underline="Underline.None">فراموشی رمز</MudLink>
        </Box>
    </MudPaper>
</Box>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Auth\Login.razor')

Write-Host 'Creating Placeholder Pages...' -ForegroundColor Green

@'
@page "/courses"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor

<MudText Typo="Typo.h4" Class="mb-2">دوره‌های آموزشی</MudText>
<MudText Typo="Typo.body1" Color="Color.Secondary">به زودی لیست کامل دوره‌ها اینجا نمایش داده می‌شود.</MudText>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Courses\Courses.razor')

@'
@page "/portfolio"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor

<MudText Typo="Typo.h4" Class="mb-2">نمونه کارها</MudText>
<MudText Typo="Typo.body1" Color="Color.Secondary">گالری آثار هنری به زودی تکمیل می‌شود.</MudText>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Portfolio\Portfolio.razor')

@'
@page "/orders"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor

<MudText Typo="Typo.h4" Class="mb-2">سفارش ساخت اثر</MudText>
<MudText Typo="Typo.body1" Color="Color.Secondary">فرم ثبت سفارش در مرحله بعد اضافه می‌شود.</MudText>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Orders\Orders.razor')

@'
@page "/profile"
@layout Marzi.Client.Shared.Layouts.MainLayout
@using MudBlazor

<MudText Typo="Typo.h4" Class="mb-2">پروفایل کاربری</MudText>
<MudText Typo="Typo.body1" Color="Color.Secondary">اطلاعات حساب و دوره‌های من اینجا نمایش داده می‌شود.</MudText>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\Pages\Profile\Profile.razor')

Write-Host 'Creating ApiSettings...' -ForegroundColor Green

@'
namespace Marzi.Client.Infrastructure;

public static class ApiSettings
{
    public const string BaseUrl = "https://localhost:7001/api/";
}
'@ | Set-Content (Join-Path $root 'Marzi.Client.Infrastructure\ApiSettings.cs')

Write-Host 'Creating App.razor...' -ForegroundColor Green

@'
<Router AppAssembly="@typeof(Marzi.Client.Shared.Layouts.MainLayout).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(Marzi.Client.Shared.Layouts.MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(Marzi.Client.Shared.Layouts.MainLayout)">
            <MudText Typo="Typo.h5">صفحه پیدا نشد</MudText>
        </LayoutView>
    </NotFound>
</Router>
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\App.razor')

Write-Host 'Creating Shared _Imports.razor...' -ForegroundColor Green

@'
@using System.Net.Http
@using Microsoft.AspNetCore.Components
@using Microsoft.AspNetCore.Components.Web
@using MudBlazor
@using Marzi.Client.Shared
@using Marzi.Client.Shared.Components.Common
@using Marzi.Client.Shared.Components.Cards
@using Marzi.Client.Shared.Components.Navigation
'@ | Set-Content (Join-Path $root 'Marzi.Client.Shared\_Imports.razor')

Write-Host 'Done! Starter Kit created successfully.' -ForegroundColor Cyan
Write-Host 'Next: Open solution and build.' -ForegroundColor Yellow