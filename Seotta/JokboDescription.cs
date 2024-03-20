﻿using System.Collections.Generic;

// 족보 도우미에 출력할 설명
public class JokboDescription
{
    private static Dictionary<string, string> descriptions = new Dictionary<string, string>
    {
        { "38광땡", "3월 광과 8월 광으로 조합된 족보입니다.\r\n\r\n모든 족보를 제압하는 가장 강력한 족보입니다." },
        
        { "암행어사", "4월 열끗과 7월 열끗 패로 조합된 족보입니다.\r\n" +
          "암행어사는 13광땡 또는 18광땡과의 대결에서 승리할 수 있습니다.\r\n\r\n" +
          "그 외의 경우에는 1끗으로 처리됩니다." },
        
        { "광땡", "1월 광과 3월 광 또는 1월 광과 8월 광으로 조합된 족보입니다.\r\n38광땡 다음으로 높은 족보입니다." },

        { "장땡", "10월 2장으로 조합된 족보입니다.\r\n\r\n일반 땡 족보 중 가장 높습니다."},

        { "멍텅구리구사", "4월 열끗과 9월 열끗 패로 조합된 족보로\r\n멍텅구리구사(또는 멍구사)라 합니다.\r\n" +
        "최종 승부에서 가장 높은 족보가 9땡 이하일 경우, 자동적으로 구사 재경기를 합니다.\r\n\r그 외의 경우에는 3끗으로 처리됩니다." },

        { "땡잡이", "3월 광과 7월 열끗 패로 조합된 족보입니다.\r\n땡잡이는 1~9땡과의 대결에서 승리할 수 있습니다.\r\n" +
        "그 외의 경우에는 최하위 족보인 망통(0끗)으로 처리됩니다." },

        { "땡", "1~9월의 같은 월 2장으로 조합된 족보입니다.\r\n\r\n월의 숫자가 높을 수록 상위 족보가 됩니다." },

        { "구사", "패 종류에 상관 없이 4월 1장과 9월 1장으로 조합된 족보입니다.\r\n" +
        "최종 승부에서 가장 높은 족보가 알리 이하일 경우, 자동적으로 구사 재경기를 합니다.\r\n\r\n" +
        "그 외의 경우에는 3끗으로 처리됩니다.\r\n" },

        { "알리", "패 종류에 상관 없이 1월 1장과 2월 1장으로 조합된 족보입니다.\r\n\r\n광땡과 땡 족보를 제외한 일반 족보 중에 가장 높은 족보입니다." },

        { "독사", "패 종류에 상관 없이 1월 1장과 4월 1장으로 조합된 족보입니다.\r\n\r\n알리보다 낮고 구삥보다 높습니다." },

        { "구삥", "패 종류에 상관 없이 9월 1장과 1월 1장으로 조합된 족보입니다.\r\n\r\n독사보다 낮고 장삥보다 높습니다." },

        { "장삥", "패 종류에 상관 없이 10월 1장과 1월 1장으로 조합된 족보입니다.\r\n\r\n구삥보다 낮고 장사보다 높습니다." },

        { "장사", "패 종류에 상관 없이 10월 1장과 4월 1장으로 조합된 족보입니다.\r\n\r\n장삥보다 낮고 세륙보다 높습니다." },

        { "세륙", "패 종류에 상관 없이 4월 1장과 6월 1장으로 조합된 족보입니다.\r\n\r\n장사보다 낮고 갑오(9끗)보다 높습니다." },

        { "갑오(9끗)", "패 종류에 상관 없이, 두 월 숫자 합의 끗수가 9인 족보입니다.\r\n\r\n끗 중에서 가장 높습니다." },

        { "끗", "패 종류에 상관 없이, 두 월 숫자 합의 끗수가 1~8인 족보입니다.\r\n\r\n끗수가 높을 수록 높은 족보입니다." },

        { "망통(0끗)", "패 종류에 상관 없이, 두 월 숫자 합의 끗수가 0인 족보입니다.\r\n\r\n모든 족보 중에 가장 낮은 족보입니다." }
    };

    public static string GetDescription(string name)
    {
        bool isKkut = name.Contains("끗") && !name.Equals("갑오(9끗)") && !name.Equals("망통(0끗)");
        bool isGwangTaeng = name.Equals("13광땡") || name.Equals("18광땡");
        bool isTaeng = name.Contains("땡") && !name.Equals("38광땡") && !name.Equals("13광땡") && !name.Equals("18광땡") && !name.Equals("장땡") && !name.Equals("땡잡이");

        // 갑오, 망통이 아닌 끗인 경우
        if (isKkut)
        {
            name = "끗";
        } // 광땡인 경우
        else if(isGwangTaeng)
        {
            name = "광땡";
        } // 땡인 경우 
        else if(isTaeng)
        {
            name = "땡";
        }

        return descriptions[name];
    }
}