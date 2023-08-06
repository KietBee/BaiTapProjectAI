#include <stdio.h>
#include <stdbool.h>

int tien[] = {1, 2, 5, 10, 20, 50, 100, 200, 500, 1000};
int numTien = sizeof(tien) / sizeof(tien[0]);

int doiTienHeuristic(int soTien, int tienCounts[]) {
    int soLuong = 0;
    for (int i = numTien - 1; i >= 0; i--) {
        if (soTien >= tien[i]) {
            int soLuongToiDa = soTien / tien[i];
            soTien -= soLuongToiDa * tien[i];
            tienCounts[i] += soLuongToiDa;
            soLuong += soLuongToiDa;
        }
    }
    return soLuong;
}

int doiTienVetCan(int soTien, int tienCounts[]) {
    if (soTien == 0) return 0;

    int minSoLuong = soTien;
    for (int i = numTien - 1; i >= 0; i--) {
        if (tien[i] <= soTien) {
            int subproblem = doiTienVetCan(soTien - tien[i], tienCounts);
            if (subproblem + 1 < minSoLuong) {
                minSoLuong = subproblem + 1;
                tienCounts[i]++;
            }
        }
    }
    return minSoLuong;
}

bool laSoTienHopLe(int soTien) {
    return soTien > 0;
}

void inKetQua(int soLuong, int tienCounts[]) {
    printf("Cach doi tien:\n");

    for (int i = numTien - 1; i >= 0; i--) {
        if (tienCounts[i] > 0) {
            printf("%d to tien menh gia %d\n", tienCounts[i], tien[i]);
        }
    }
}

int main() {
    int soTien;
    printf("Nhap so tien can doi: ");
    scanf("%d", &soTien);

    if (!laSoTienHopLe(soTien)) {
        printf("So tien khong hop le. Vui long nhap mot so nguyen duong.\n");
        return 1;
    }

    printf("So tien can doi: %d\n", soTien);

	int tienCountsVetCan[numTien] = {0};
    int soLuongVetCan = doiTienVetCan(soTien, tienCountsVetCan);
    printf("Thuat toan Vet Can: So to tien it nhat can doi %d to tien\n", soLuongVetCan);
    
    int tienCountsHeuristic[numTien] = {0};
    int soLuongHeuristic = doiTienHeuristic(soTien, tienCountsHeuristic);
    printf("Thuat toan Heuristic: So to tien it nhat can doi %d to tien\n", soLuongHeuristic);

    inKetQua(soLuongHeuristic, tienCountsHeuristic);

    return 0;
}
