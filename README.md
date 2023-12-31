# Программа для добавления разрешений для работы с микрофном и камерой в макОС

## Контекст

После установки MacOS Ventura на старый мак пропала возможность выдавать пермиссии приложениям. Единственный способ их добавить был ручное добавление посредством выполнения серии скриптов. 
Собственно для автоматизации этого процесса и была написана данная программа.


## Использование

Необходимо выполнить следующие шаги для начала работы с программой:
1. Установить **.NET 7**
2. Скачать архив с программой из раздела **Releases** 
3. Поместить разархивированную программу в желаемый раздел файловой системы;
4. Командой `chmod +x tccplus` в директории программы разрешить исполнение процедуры добавления пермиссии (*требуется выполнить на конкретной машине только один раз*);
5. Запустить ее командой `./PermissionApp default  <<your_app_path>>` (***your_app_path** проще всего получить путем перетягивания в терминал приложения из раздела приложений*);
6. Вы красавчик :)

В данном варианте выполнения программа добавляет разрешения на работу с микрофоном, камерой и захватом экрана. 
Программа также позволяет указать произвольное количество других опций для добавления. Более подробную информацию можно получить, 
выполнив команду `./PermissionApp --help `

## Источники вдохновения

Программа сделана по следующему руководству:
https://superuser.com/questions/1779925/macos-ventura-cant-give-permissions-opencore