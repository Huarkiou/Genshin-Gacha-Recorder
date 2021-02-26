### 大致流程
1. 打开原神并进入卡池历史记录页面
2. 读取文件${userPath}/AppData/LocalLow/miHoYo/${name}/output_log.txt
在文件中找到`OnGetWebViewPageFinish:OnGetWebViewPageFinish:https://webstatic.mihoyo.com/hk4e/event/e20190909gacha/index.html?authkey_ver=1&sign_type=2&auth_appid=webview_gacha&init_type=301&gacha_id=a3101d03239e0eb2a79c4b144c5197b296f087&lang=zh-cn&device_type=pc&ext=%7b%22loc%22%3a%7b%22x%22%3a666.0570068359375%2c%22y%22%3a155.1297607421875%2c%22z%22%3a1167.5489501953125%7d%2c%22platform%22%3a%22WinST%22%7d&game_version=CNRELWin1.3.2_R1994426_S2006143_D2037460&region=cn_gf01&authkey=blpJDLhM9MDtYPlx27iLTYHZd4omEsCUfhixNsJgHf3q7pOLxRZPA4M%2bnJninpJt0Cxg80keEUgFIDUhAYUvsIDsY8BLlWhnEtf9Lw%2fKpRlt5ugpqOwVVyxzaA8%2bjgKHZ6aQ9pMyyTUbpj2holDl1Rgwi%2fU5WObXTsS4pR6yLGvuEl4HevTXArQRuM30jCtFTISPk23X7JphDlaRvAc7dXyCV4bW6MxHLxmZGft3sNvv1FGFgqYjwJSz8nuejgPdLuXqFEZYuw0lHX0lc6iWLp%2bqTViXyf%2fekY8p0IS6yVo31IA%2fxC5hBp%2bXjTFwiEUX%2fLxbjVHiQsr6w4PJvV6qq%2bqF6W6UsmU3jOnp3vd9lU3Q0FGODDJa%2f%2byDDios0rgIYD16iAEZVmr7mg9j4mEUwaGYfUxa6wxpmuWOYVwtOEnNfd20jJ1NZT5U5KgYjr8X0nC%2bCAO8Ygi4bVomK3DL3p434EOEulKneIuX982s3miivzcaypWlEZLpEZ5DvMC9hdIZm3EcdVP2o46WDB2%2f9gCD2D9yBFQ49xmq25yrlZaNLPZMrmXKO3HaBi5s9WhBUvouScmt%2f26gh4mYKVFBnSeObzuk1Fqh9UENu3oXuvBFvF6xkn1C6Z177Xm228e8pHAsIrq4SpRdQD6mHd6YpycwhM8RINFMJu52CxKXQ%2fOb4Fw2Lq0vTy9yxX8mcEnggaphIByTKQ9mgFU8k20pfD4n%2fyERKKfxfJ2N%2feeiFxCR0GTVkOteI30Da2PIIA2kL8wExJ7BdA4gGv9lM8R6iJwNyJHfGGEFhPxJ0q8y1BdreqLhkHQ46d2Jxhl8C1HUtAonjyzpHsXjdtmKBx2DO1sfvc%2b4pBbkAJ0Pk652rOybqrxFWu9eU%2b%2bDGpPnv1UWiPKW6orDSWDU5pyDu0WSHq%2fvdQGvMZ07nikmElBOa6%2f70gmHw8IKn8o%2fdgZOHYyGbzY9cXkoa%2bw%2blsiv%2bYmwBTSJ0YPYKxhg6iuoIcVgBDzDZTuISlQ8ALsu8FVLYpA6&game_biz=hk4e_cn#/log`
3. 访问步骤2中得到的网址即为祈愿历史记录页面
通过GET请求https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?authkey_ver=1&sign_type=2&auth_appid=webview_gacha&init_type=200&gacha_id=a3101d03239e0eb2a79c4b144c5197b296f087&lang=zh-cn&device_type=pc&ext=%7b%22loc%22%3a%7b%22x%22%3a666.0570068359375%2c%22y%22%3a155.1297607421875%2c%22z%22%3a1167.5489501953125%7d%2c%22platform%22%3a%22WinST%22%7d&game_version=CNRELWin1.3.2_R1994426_S2006143_D2037460&region=cn_gf01&authkey=blpJDLhM9MDtYPlx27iLTYHZd4omEsCUfhixNsJgHf3q7pOLxRZPA4M%2bnJninpJt0Cxg80keEUgFIDUhAYUvsIDsY8BLlWhnEtf9Lw%2fKpRlt5ugpqOwVVyxzaA8%2bjgKHZ6aQ9pMyyTUbpj2holDl1Rgwi%2fU5WObXTsS4pR6yLGvuEl4HevTXArQRuM30jCtFTISPk23X7JphDlaRvAc7dXyCV4bW6MxHLxmZGft3sNvv1FGFgqYjwJSz8nuejgPdLuXqFEZYuw0lHX0lc6iWLp%2bqTViXyf%2fekY8p0IS6yVo31IA%2fxC5hBp%2bXjTFwiEUX%2fLxbjVHiQsr6w4PJvV6qq%2bqF6W6UsmU3jOnp3vd9lU3Q0FGODDJa%2f%2byDDios0rgIYD16iAEZVmr7mg9j4mEUwaGYfUxa6wxpmuWOYVwtOEnNfd20jJ1NZT5U5KgYjr8X0nC%2bCAO8Ygi4bVomK3DL3p434EOEulKneIuX982s3miivzcaypWlEZLpEZ5DvMC9hdIZm3EcdVP2o46WDB2%2f9gCD2D9yBFQ49xmq25yrlZaNLPZMrmXKO3HaBi5s9WhBUvouScmt%2f26gh4mYKVFBnSeObzuk1Fqh9UENu3oXuvBFvF6xkn1C6Z177Xm228e8pHAsIrq4SpRdQD6mHd6YpycwhM8RINFMJu52CxKXQ%2fOb4Fw2Lq0vTy9yxX8mcEnggaphIByTKQ9mgFU8k20pfD4n%2fyERKKfxfJ2N%2feeiFxCR0GTVkOteI30Da2PIIA2kL8wExJ7BdA4gGv9lM8R6iJwNyJHfGGEFhPxJ0q8y1BdreqLhkHQ46d2Jxhl8C1HUtAonjyzpHsXjdtmKBx2DO1sfvc%2b4pBbkAJ0Pk652rOybqrxFWu9eU%2b%2bDGpPnv1UWiPKW6orDSWDU5pyDu0WSHq%2fvdQGvMZ07nikmElBOa6%2f70gmHw8IKn8o%2fdgZOHYyGbzY9cXkoa%2bw%2blsiv%2bYmwBTSJ0YPYKxhg6iuoIcVgBDzDZTuISlQ8ALsu8FVLYpA6&game_biz=hk4e_cn&gacha_type=200&page=4&size=6&lang=zh-cn
得到相应页码的祈愿数据

GachaTypesUrl = `https://hk4e-api.mihoyo.com/event/gacha_info/api/getConfigList?${queryString}`
--->
0: {id: "4", key: "200", name: "常驻祈愿"}
1: {id: "14", key: "100", name: "新手祈愿"}
2: {id: "15", key: "301", name: "角色活动祈愿"}
3: {id: "16", key: "302", name: "武器活动祈愿"}

GachaLogBaseUrl = `https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?authkey_ver=1&sign_type=2&auth_appid=webview_gacha&init_type=301&gacha_id=a3101d03239e0eb2a79c4b144c5197b296f087&lang=zh-cn&device_type=pc&ext=%7b%22loc%22%3a%7b%22x%22%3a1638.618896484375%2c%22y%22%3a197.40589904785157%2c%22z%22%3a-2666.962158203125%7d%2c%22platform%22%3a%22WinST%22%7d&game_version=CNRELWin1.3.2_R1994426_S2006143_D2063407&region=cn_gf01&authkey=blpJDLhM9MDtYPlx27iLTYHZd4omEsCUfhixNsJgHf3q7pOLxRZPA4M%2bnJninpJt0Cxg80keEUgFIDUhAYUvsIDsY8BLlWhnEtf9Lw%2fKpRlt5ugpqOwVVyxzaA8%2bjgKHZ6aQ9pMyyTUbpj2holDl1Rgwi%2fU5WObXTsS4pR6yLGvuEl4HevTXArQRuM30jCtFTISPk23X7JphDlaRvAc7dXyCV4bW6MxHLxmZGft3sNvv1FGFgqYjwJSz8nuejgPdLuXqFEZYuw0lHX0lc6iWLp%2bqTViXyf%2fekY8p0IS6yVo31IA%2fxC5hBp%2bXjTFwiEUX%2fLxbjVHiQsr6w4PJvV6qq%2bqF6W6UsmU3jOnp3vd9lU3Q0FGODDJa%2f%2byDDios0rgIYD16iAEZVmr7mg9j4mEUwaGYfUxa6wxpmuWOYVwtOEnNfd20jJ1NZT5U5KgYjr8X0nC%2bCAO8Ygi4bVomK3DL3p434EOEulKneIuX982s3miivzcaypWlEZLpEZ5DvMC9hdIZm3EcdVP2o46WDB2%2f9gCD2D9yBFQ49xmq25yrlZaNLPZMrmXKO3HaBi5s9WhBUvouScmt%2f26gh4mYKVFBnSeObzuk1Fqh9UENu3oXuvBFvF6xkn1C6Z177Xm228e8pHAsIrq4SpRdQD6mHd6YpycwhM8RINFMJu52CxKXQ%2fPK1u9w3Rtx%2fHohEyJN%2fknlDHHHpHVZuU%2fM4vq6srpU7ScONuIMSf8niGu9t55HrKXGMMxEwhvCu6HQd4TOD%2f84A6bQzuRlgpJdDZI71ygU2CuoM5cRM4oLWjV316DuKvIA%2fdPapd6A1dSTMSOBuNKX7uInIcmFRvnww7l2Uws3qyUnhABg%2b8kK9bBliN6xP%2fezQukxlhDhC%2bqfBs0%2bt%2bx7%2bRLyLU7BnPYHAQfylSB8KzgiFTXvPVA7cIs%2fjEoRFYEjFQBmVayNU%2ftV%2fmelpIbf9EPpNq6OckwiO1wpR6QfYR4yrNnjJuOslkzT7GrHBZ8HyUjYo%2fgoYLDPMPZhU4kU&game_biz=hk4e_cn
(GachaLogBaseUrl + `&gacha_type=${key}` + `&page=${page}` + `&size=${20}`)
--->
0: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-29 00:05:08",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "讨龙英杰谭"
rank_type: "3"
time: "2021-01-29 00:05:08"
uid: "100692857"
1: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-20 17:20:20",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "魔导绪论"
rank_type: "3"
time: "2021-01-20 17:20:20"
uid: "100692857"
2: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-20 17:16:31",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "流浪乐章"
rank_type: "4"
time: "2021-01-20 17:16:31"
uid: "100692857"
3: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-20 16:45:40",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "飞天御剑"
rank_type: "3"
time: "2021-01-20 16:45:40"
uid: "100692857"
4: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-11 16:54:17",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "西风猎弓"
rank_type: "4"
time: "2021-01-11 16:54:17"
uid: "100692857"
5: {uid: "100692857", gacha_type: "200", item_id: "", count: "1", time: "2021-01-11 16:54:01",…}
count: "1"
gacha_type: "200"
item_id: ""
item_type: "武器"
lang: "zh-cn"
name: "黑缨枪"
rank_type: "3"
time: "2021-01-11 16:54:01"
uid: "100692857"

反复请求直到全部读取