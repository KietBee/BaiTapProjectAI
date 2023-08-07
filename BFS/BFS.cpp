#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>

#define MaxN 100001

int n;
int mark[MaxN];
int adj[MaxN][MaxN];

typedef struct {
    int data[MaxN];
    int front, rear;
} Queue;

void initQueue(Queue *q) {
    q->front = q->rear = -1;
}

bool isEmpty(Queue *q) {
    return q->front == -1;
}

void enqueue(Queue *q, int value) {
    if (isEmpty(q)) {
        q->front = q->rear = 0;
    } else {
        q->rear++;
    }
    q->data[q->rear] = value;
}

int dequeue(Queue *q) {
    int value = q->data[q->front];
    if (q->front == q->rear) {
        q->front = q->rear = -1;
    } else {
        q->front++;
    }
    return value;
}

void BFS() {
    Queue q;
    initQueue(&q);
    enqueue(&q, 1);

    while (!isEmpty(&q)) {
        int u = dequeue(&q);
        mark[u] = 1;
        printf("%d ", u);

        for (int v = 1; v <= n; v++) {
            if (adj[u][v] && !mark[v]) {
                enqueue(&q, v);
                mark[v] = 1;
            }
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

    BFS();

    return 0;
}
