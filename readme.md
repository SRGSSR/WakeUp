# Ziel und Zweck   
Diese Dokumentation beschreibt die Implementierung und den Verwendungszweck des WakeupTools, welches für TPC entwickelt wurde.  
 
# Allgemeine Informationen 
## Verwendungszweck  
TPC muss auf allen Büro Clients den Bildschirmschoner aktivieren. Nun gibt es im Client Umfeld zahlreiche Applikationen, welche mit dem Bildschirmschoner nicht umgehen können oder sogar ganz abstürzen. Ziel war es ein Tool zu programmieren, welches den Bildschirmschoner verhindert, wenn eines dieser kritischen Applikationen läuft.  

Workplace Services entwickelte ein Tool, welches anhand eines zentral gespeicherten XML Files die Applikationen erkennt, welche nicht mit dem Bildschirmschoner umgehen können. Das Wakeup Tool überprüft laufend, ob auf dem Client einer dieser kritischen Applikationen läuft. Falls die Applikation gefunden wurde, wird der Bildschirmschoner unterdrückt.  
  
Sobald wiederum die kritische Applikation beendet wird, wird auch der Bildschirmschoner des Clients nicht weiter unterdrückt.  

# Verwendete Tools 
Programmiersprache: C#, .Net

Entwicklungsumgebung: Visual Studio Community, OpenSource   

Involvierte Systeme:  
* IIS Webserver als Provider für das zentrale XML File: gethelp.media.int 
 * http://gethelp.media.int/public/configuredProcesses.xml  
 * Share Zugriff: \\tpcs-bechu-0016\gethelp$  
* Client: tpc Windows 7 Basis Client 
 
# Implementierung 
Die Funktionsweise der Applikation ist besser im PDF erklärt.  

Es wurde eine Windowsform Applikation erstellt. Die Main Klasse ist die Programm.cs diese startet anschliessend das Windows Formular. 
 
## Erklärung der einzelnen Klassen 
### WakeupSettings.settings 
 * Definiert die Settings für die Applikation
 * url: http://gethelp.media.int/public/configuredProcesses.xml 
  * URL für das XML in welchem die kritischen Applikationen definiert sind 
 * delayIntervallInMinutes: 60 
  * Alle x Minuten bei welchem das Wakeup Tool das XML neu herunterlädt 
 * delayIntervallInSeconds: 60 
  * Lokale refresh Intervall der momentan laufenden Prozesse 
 * configPath: %TEMP%\configuredProcesses.xml 
  * lokaler Pfad für das XML File 

## Programm.cs 
 * Main Methode für das Ausführen der Applikation 

## WakeupForm.cs 
 * Definiert auf der einen Seite das Formular. Dieses stellt das TrayIcon zur Verfügung.  
 * Der Code, welcher mit dem Formular verbunden ist, behandelt die zentrale Logik der Applikation 

## EventLogger.cs:
 * Definiert wie mit Events in der Applikation umgegangen wird. Momentan wird die Applikation bei einem Fehler beendet, welche nicht gecatcht wurden. 

## XMLHandler:
* bietet die Möglichkeit das XML vom Webserver zu laden. Dies wird als Task alle x min durchgeführt. Falls das File vom Webserver nicht geladen werden konnte, wird das lokale XML zurückgegeben.  

# Paketierung 
Das beim builden der Applikation entstehende exe und settings File wird mithilfe eines Columbus Pakets in folgenden Pfad kopiert: C:\Program Files\tpc\WakeUp\WakeUpGui.exe 

Zudem wird eine Verknüpfung in den Autostart des Benutzers angelegt. Die Applikation kann nicht in den Autostart der Registry hinzugefügt werden, da in diesem Zeitpunkt noch nicht alle notwendigen Komponenten geladen wurden.  

# Prozess  
Die Applikationen welche den Sleep verhindern dürfen, also ins XML aufgenommen werden müssen, müssen durch den lokalen ISIBE bewilligt werden.  
Es muss ein Incidient vorliegen, welcher beweist, dass die Applikation nicht mit dem Sleep umgehen kann.  

# Rollout 
Vor dem Rollout der Policy Einstellung wird die Applikation auf alle Clients im TPC verteilt. Dije Applikation verhindert zu diesem Zeitpunkt bereits den Sleep. Jedoch hat dies noch keine Auswirkung auf den Client, da dieser noch keinen Sleep konfiguriert hat.  

Die Policy wird dann über eine Policy auf die Clients verteilt. Um den Rollout gestaffelt durchzuführen wird die Policy über eine Security Gruppe gefiltert.  
