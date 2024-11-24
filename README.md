# Chess

Second attempt at creating a chess engine.

## Notes

- Copying boards is faster than move/undo. It can also be parallelised.
- Careful parallelising; combined with pruning it could miss good paths.

## Reminders to Self

- Make anything other than a minor change in a branch. Check against perft before merging.

## Expected Results

Number of possible board combinations after each move (without pruning).

```
+-----------------|---------------------------+
| Number of moves | Number of possible states |
| 1               | 20                        |
| 2               | 400                       |
| 3               | 8,902                     |
| 4               | 197,281                   |
| 5               | 4,865,609                 |
| 6               | 119,060,324               |
| 7               | 3,195,901,860             |
| 8               | 84,998,978,956            |
| 9               | 2,439,530,234,167         |
| 10              | 69,352,859,712,417        |
+-----------------+---------------------------+
```

## TODOs

- Pawn promotion (WIP), also account for promotions when scoring a move
- Stalemate detection
- Fix crash on (or near) checkmate
- Threefold repetition
- Ranking move quality
- Pruning
- Move ordering (prefer captures, ideally ordered by lowest value catches highest value first)
- LiChessClient thinks a stalemate is a win for the opponent
- LiChessClient thinks opponent wins when it did
- LiChessClient thinks opponent wins when opponent did (at least that's correct)
- Engine generates moves like "e1f-" sometimes?

## Useful links

- https://www.chessprogramming.org/Perft_Results
- https://database.lichess.org/