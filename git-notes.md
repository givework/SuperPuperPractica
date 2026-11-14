* fbdf3c52 (HEAD -> main, origin/main, origin/master) разрешен конфликт в README.md
|\
| * ec757f0a (feature/git-basics) Обновлён заголовок README в feature
* | 6b4e517c Обновлён заголовок README в main
|/
* 74c4a18 выполнение 15 задания
* 0848c04 Создание структуры и логики с оформлением меню и выполнение 14 заданий из 1 уровня сложности
* faf79088 Update .gitignore for requirements
* 7d980ae9 Проверка на работу коммита
* 66e724f9 Добавьте файлы проекта.
* 6bebea91 Добавить .gitattributes и .gitignore.

=== ЗАДАНИЕ 46: GIT STASH ===

--- ДО (git stash list) ---
(список пуст)

--- ДЕЙСТВИЯ ---
1. Внесены изменения в Program.cs (добавлена тестовая строка)
2. git stash - изменения спрятаны
   Результат: Saved working directory and index state WIP on master
3. git stash list - проверка
   Результат: stash@{0}: WIP on master: c111e2e Merge branch 'master'
4. Переключение на ветку feature/stash-test и возврат на master
5. git stash pop - изменения восстановлены
   Результат: Changes not staged for commit: modified: Practica/Program.cs
   Dropped refs/stash@{0}

--- ПОСЛЕ (git stash list) ---
(список пуст - stash применен и удален)

=== ЗАДАНИЕ 47: REBASE -i (SQUASH) ===

--- ДО rebase (4 коммита) ---
7e0650d add: fourth line
0d7d8bd add: third line
3009590 add: second line
36c8dcd add: first line

--- ПОСЛЕ rebase (1 коммит) ---
f905d23 feat: add 4 lines to squash-demo.txt

=== ЗАДАНИЕ 48: GIT ТЕГИ (v1.0.0) ===

--- git tag ---
v1.0.0

--- git show v1.0.0 ---
tag v1.0.0
Tagger: Scratch_x <email>
Date:   Sun Nov 14 22:07:43 2026

Release v1.0.0

commit f905d23...
Author: Scratch_x
Date:   Sun Nov 13 18:59:32 2026

=== ЗАДАНИЕ 49: ВОССТАНОВЛЕНИЕ ЧЕРЕЗ REFLOG ===

--- git reflog ---
5fc7382 HEAD@{0}: reset: moving to 5fc7382
c111e2e HEAD@{1}: reset: moving to HEAD~1
5fc7382 HEAD@{2}: commit: feat: add reflog test file
c111e2e HEAD@{3}: checkout: moving from master to master
c111e2e HEAD@{4}: checkout: moving from feature/squash-demo to master
f905d23 HEAD@{5}: rebase (finish): returning to refs/heads/feature/squash-demo
f905d23 HEAD@{6}: commit (amend): feat: add 4 lines to squash-demo.txt

--- git log --oneline ---
5fc7382 (HEAD -> master) feat: add reflog test file
c111e2e (tag: v1.0.0, origin/master, origin/HEAD, feature/stash-test) docs: add task 48 tag demo
f905d23 (feature/squash-demo) feat: add 4 lines to squash-demo.txt



=== ЗАДАНИЕ 50: СИМУЛЯЦИЯ GIT BISECT ===

--- git bisect log ---
git bisect start
bad: HEAD fix: calculator v8
git bisect bad
good: [HEAD7] fix: calculator v1
git bisect good HEAD~7
bad: [HEAD3] fix: calculator v5
git bisect bad
good: [HEAD5] fix: calculator v3
git bisect good
bad: [HEAD4] fix: calculator v4 (BROKEN)
git bisect bad
first bad commit: [HEAD4] fix: calculator v4 (BROKEN)

--- Результат ---
Плохой коммит найден: v4 (BROKEN)
В нём метод возвращает 1+1=5 вместо 1+1=2

--- git bisect reset ---
HEAD is now at feature/bisect-demo
