[CmdletBinding()]
Param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

dotnet pack --output "./../.artifacts" --configuration Release -p:PackageVersion=$Version $ScriptArgs