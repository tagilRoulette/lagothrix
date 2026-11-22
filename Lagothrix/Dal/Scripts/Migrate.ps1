$ErrorActionPreference = 'Stop'

# Переходим в корень решения относительно расположения самого скрипта,
# а не относительно текущей рабочей директории вызывающего.
Set-Location (Join-Path $PSScriptRoot '../..') -ErrorAction Stop

function Invoke-Dotnet {
    param(
        [Parameter(Mandatory)][string]   $Name,
        [Parameter(Mandatory)][string[]] $Arguments
    )

    Write-Host "==> $Name" -ForegroundColor Cyan
    dotnet @Arguments

    # Ключевая проверка: падение dotnet (в т.ч. встроенной сборки
    # перед генерацией миграции) не даст скрипту пойти дальше.
    if ($LASTEXITCODE -ne 0) {
        throw "Шаг '$Name' завершился с кодом $LASTEXITCODE"
    }
}

try {
    # Явная сборка — чтобы упасть сразу, до первой миграции.
    # Если у вас есть .sln, замените 'Api' на путь к нему.
    Invoke-Dotnet 'Сборка' @('build', 'Lagothrix.sln')

    $migrations = @(
        @{ Name = 'Users';             Context = 'UsersDbContext' }
        @{ Name = 'Lots';              Context = 'LotsDbContext' }
        @{ Name = 'LotImages';         Context = 'LotImagesDbContext' }
        @{ Name = 'Categories';        Context = 'CategoriesDbContext' }
        @{ Name = 'ExchangeOffers';    Context = 'ExchangeOffersDbContext' }
        @{ Name = 'AiEstimatedWorths'; Context = 'AiEstimatedWorthsDbContext' }
        @{ Name = 'Parameters';        Context = 'ParametersDbContext' }
        @{ Name = 'ParameterValues';   Context = 'ParameterValuesDbContext' }
    )

    foreach ($m in $migrations) {
        Invoke-Dotnet "Миграция $($m.Name)" @(
            'ef', 'migrations', 'add', $m.Name,
            '--context', $m.Context,
            '--output-dir', 'Migrations',
            '--project', 'Dal',
            '--startup-project', 'Api'
        )
    }

    Write-Host 'Все миграции созданы.' -ForegroundColor Green
    exit 0
}
catch {
    Write-Error "Скрипт остановлен: $($_.Exception.Message)"
    exit 1
}