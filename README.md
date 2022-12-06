## Условие задачи

Создать решение, которое максимально быстро считает статистические параметры по котировкам с биржи.  
Для реализации необходимо создать 2 консольных приложения.

### 1-е приложение-сервер 

Бесконечно генерирует случайные числа в диапазоне (для эмуляции предметной области - потока котировок с биржи), рассылает мультикастом по
udp, без задержек.  

Диапазон и мультикаст -группа настраивается через отдельный xml конфиг.

### 2-е приложение-клиент
Принимает данные по udp, считает по всем полученным: среднее арифметическое, стандартное отклонение, моду и медиану.  
Общее количество полученных пакетов может быть от триллиона и выше.

Посчитанные значения выводит в консоль по требованию (нажатие энтер).

Приложение должно контролировать получение всех пакетов, количество потерянных пакетов должно выводиться совместно со статистикой (для форсирования потери пакетов нужно добавить задержку в прием сообщений,
примерно один раз в секунду).

Прием пакетов и счет реализовать в разных потоках с минимальными задержками.

Мультикаст-группа и задержка приема должна настраиваться через отдельный xml конфиг (не в app.config).

**Важное требование**: приложение должно быть максимально оптимизировано по скорости
работы с учетом объема полученных данных и выдавать решение как можно быстрее (в
течении нескольких миллисекунд) - для бирж значение имеет каждая микросекунда.

Приложение должно работать продолжительное время (неделя-месяц) без падений по
внутренним ошибками, а также в случае ошибок в сети.
