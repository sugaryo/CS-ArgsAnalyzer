# 取り敢えずREADMEを用意した

## TabCompletion

### Tab入力補完

bash の `bash-completion` みたいな `TAB` キーでの入力補完機能を作った。  
`InputWizzard` に `TabCompletion` を組み込み。

### FunctionKey選択

前方一致候補リストに `[F1] ～ [F12]` のファンクションキー選択機能を追加実装。

### feature

> "A" + [TAB] + "B" + [TAB] + "C" + [TAB]

って入力するのがダルいので

> "ABC" + [END]

って入力すれば上記と同等動作になるような、ショートカット入力がしたい。
