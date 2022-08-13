# 取り敢えずREADMEを用意した

## コンソール入力ウィザード

### 真偽値対話

単純な `Yes/No` 設問で振り分け出来るように、対話機能を追加。

## TabCompletion

### Tab入力補完

bash の `bash-completion` みたいな `TAB` キーでの入力補完機能を作った。  
`InputWizzard` に `TabCompletion` を組み込み。

### End入力補完（連続Tab補完）

> "A" + [TAB] + "B" + [TAB] + "C" + [TAB]

って入力するのがダルいので

> "ABC" + [END]

って入力すれば上記と同等動作になるような、ショートカット入力機能を追加実装。

### FunctionKey選択

前方一致候補リストに `[F1] ～ [F12]` のファンクションキー選択機能を追加実装。
