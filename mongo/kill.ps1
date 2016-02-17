net stop "mongodb"
$service = Get-WmiObject -Class Win32_Service -Filter "Name='mongodb'"
$service.delete()