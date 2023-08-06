#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>

#define MaxN 100001

int n;
int mark[MaxN];
int adj[MaxN][MaxN];

void DFS(int u) {
    printf("%d ", u);
    mark[u] = 1;
    for (int v = 1; v <= n; v++) {
        if (adj[u][v] && !mark[v]) {
            DFS(v);
        }
    }
}

int main() {
    freopen("CTDL.inp", "r", stdin);
    freopen("CTDL.out", "w", stdout);
    scanf("%d", &n);

    for (int i = 0; i < n - 1; i++) {
        int u, v;
        scanf("%d %d", &u, &v);
        adj[u][v] = adj[v][u] = 1;
    }

    DFS(1);

    return 0;
}
