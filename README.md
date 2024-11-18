# Chess

Second attempt at creating a chess engine.

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

- En passant
- Castling (not castling through check)
- Pawn promotion
- Move cannot leave king in check
- Stalemate detection