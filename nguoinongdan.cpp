#include <stdio.h>
#include <stdbool.h>
#include <string.h>

#define MAX_STATES 100

typedef enum { w, e } Location;

typedef struct {
    Location farmer, wolf, goat, cabbage;
} State;

bool isValid(State s) {
    if ((s.goat == s.cabbage && s.farmer != s.goat) || (s.wolf == s.goat && s.farmer != s.goat)) {
        return false;
    }
    return true;
}

void printState(State s) {
    printf("(%c, %c, %c, %c)\n",
           s.farmer == w ? 'W' : 'E',
           s.wolf == w ? 'W' : 'E',
           s.goat == w ? 'W' : 'E',
           s.cabbage == w ? 'W' : 'E');
}

bool isVisited(State s, State visitedStates[], int numVisited) {
    for (int i = 0; i < numVisited; i++) {
        if (memcmp(&s, &visitedStates[i], sizeof(State)) == 0) {
            return true;
        }
    }
    return false;
}

int heuristic(State s) {
    int remainingObjects = (s.farmer == w) + (s.wolf == w) + (s.goat == w) + (s.cabbage == w);
    return remainingObjects;
}

void solve(State currentState, State visitedStates[], int *numVisited) {
    State finalState = {e, e, e, e};

    if (memcmp(&currentState, &finalState, sizeof(State)) == 0) {
        printState(currentState);
        return;
    }

    int minPriority = MAX_STATES + 1;
    int minPriorityIdx = -1;

    for (int i = 0; i < 4; i++) {
        State nextState = currentState;
        nextState.farmer = nextState.farmer == w ? e : w;

        switch (i) {
            case 0: nextState.wolf = nextState.wolf == w ? e : w; break;
            case 1: nextState.goat = nextState.goat == w ? e : w; break;
            case 2: nextState.cabbage = nextState.cabbage == w ? e : w; break;
            case 3: break;
        }

        if (isValid(nextState) && !isVisited(nextState, visitedStates, *numVisited)) {
            int h = heuristic(nextState);
            int priority = h;  // Chỉ sử dụng heuristic làm mức ưu tiên
            if (priority < minPriority) {
                minPriority = priority;
                minPriorityIdx = i;
            }
        }
    }

    if (minPriorityIdx != -1) {
        State nextState = currentState;
        nextState.farmer = nextState.farmer == w ? e : w;

        switch (minPriorityIdx) {
            case 0: nextState.wolf = nextState.wolf == w ? e : w; break;
            case 1: nextState.goat = nextState.goat == w ? e : w; break;
            case 2: nextState.cabbage = nextState.cabbage == w ? e : w; break;
            case 3: break;
        }

        printState(nextState);
        visitedStates[(*numVisited)++] = nextState;
        solve(nextState, visitedStates, numVisited);
    }
}

int main() {
    State initialState = {w, w, w, w};
    State visitedStates[MAX_STATES];
    int numVisited = 0;

    printf("Cac buoc di chuyen cua nguoi nong dan qua song:\n");
    printState(initialState);

    visitedStates[numVisited++] = initialState;

    solve(initialState, visitedStates, &numVisited);

    return 0;
}
