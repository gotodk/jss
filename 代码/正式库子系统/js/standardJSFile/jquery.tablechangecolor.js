

(function ($) {
    /*
    Jquery对象级别插件(基于框架的)
    */
    $.fn.tablechangecolor = function (options) {
        $.fn.tablechangecolor.defaults =
    {
        hoverColor: "#efefef",
        clickColor: "#d6e3f3"
    };
        // default options used on initialisation
        // and arguments used on later calls
        var opts = $.extend({}, $.fn.tablechangecolor.defaults, options);
        var args = arguments;

        /*
        * Entry point
        */
        return this.each(function () {
            if ($(this).get(0).tagName == "TABLE") {
                $(this).children("tbody").each(function () {
                    $(this).children("tr").each(function () {
                        $(this).bind("click", function () {

                            $(this).parent().each(function () {
                                $(this).children("tr").each(function () {
                                    $(this).get(0).style.backgroundColor = '';
                                });
                            });
                            $(this).get(0).style.backgroundColor = opts.clickColor;
                        });
                        $(this).bind("mouseover", function () {

                            if ($.ColorChange($(this).get(0).style.backgroundColor) != opts.clickColor) {
                                $(this).get(0).style.backgroundColor = opts.hoverColor;
                            }
                        });
                        $(this).bind("mouseout", function () {
                        
                            if ($.ColorChange($(this).get(0).style.backgroundColor) != opts.clickColor) {
                                $(this).get(0).style.backgroundColor = '';
                            }
                        });
                    });
                });
            }
        });
    }
    /*
    类级别的Jquery插件
    */
    $.extend({
        ColorChange: function (colorFormat) {
            if (colorFormat.indexOf("#") == -1 && colorFormat != "") {

                var re = /(rgb+)|(\(+)|(\)+)/g;
                colorFormat = colorFormat.replace(re, "");
                var hexArr = colorFormat.split(',');
                colorFormat = $.GetColorFormat(parseInt(hexArr[0]), parseInt(hexArr[1]), parseInt(hexArr[2]));

            }
            return colorFormat;
        },
        GetColorFormat: function (red, green, blue) {
            /*颜色数组*/
            var hexarray = new Array(256);
            hexarray[0] = "00"; hexarray[1] = "01"; hexarray[2] = "02";
            hexarray[3] = "03"; hexarray[4] = "04"; hexarray[5] = "05";
            hexarray[6] = "06"; hexarray[7] = "07"; hexarray[8] = "08";
            hexarray[9] = "09"; hexarray[10] = "0a"; hexarray[11] = "0b";
            hexarray[12] = "0c"; hexarray[13] = "0d"; hexarray[14] = "0e";
            hexarray[15] = "0f"; hexarray[16] = "10"; hexarray[17] = "11";
            hexarray[18] = "12"; hexarray[19] = "13"; hexarray[20] = "14";
            hexarray[21] = "15"; hexarray[22] = "16"; hexarray[23] = "17";
            hexarray[24] = "18"; hexarray[25] = "19"; hexarray[26] = "1a";
            hexarray[27] = "1b"; hexarray[28] = "1c"; hexarray[29] = "1d";
            hexarray[30] = "1e"; hexarray[31] = "1f"; hexarray[32] = "20";
            hexarray[33] = "21"; hexarray[34] = "22"; hexarray[35] = "23";
            hexarray[36] = "24"; hexarray[37] = "25"; hexarray[38] = "26";
            hexarray[39] = "27"; hexarray[40] = "28"; hexarray[41] = "29";
            hexarray[45] = "2d"; hexarray[46] = "2e"; hexarray[47] = "2f";
            hexarray[48] = "30"; hexarray[49] = "31"; hexarray[50] = "32";
            hexarray[51] = "33"; hexarray[52] = "34"; hexarray[53] = "35";
            hexarray[54] = "36"; hexarray[55] = "37"; hexarray[56] = "38";
            hexarray[57] = "39"; hexarray[58] = "3a"; hexarray[59] = "3b";
            hexarray[60] = "3c"; hexarray[61] = "3d"; hexarray[62] = "3e";
            hexarray[63] = "3f"; hexarray[64] = "40"; hexarray[65] = "41";
            hexarray[66] = "42"; hexarray[67] = "43"; hexarray[68] = "44";
            hexarray[69] = "45"; hexarray[70] = "46"; hexarray[71] = "47";
            hexarray[72] = "48"; hexarray[73] = "49"; hexarray[74] = "4a";
            hexarray[75] = "4b"; hexarray[76] = "4c"; hexarray[77] = "4d";
            hexarray[78] = "4e"; hexarray[79] = "4f"; hexarray[80] = "50";
            hexarray[81] = "51"; hexarray[82] = "52"; hexarray[83] = "53";
            hexarray[84] = "54"; hexarray[85] = "55"; hexarray[86] = "56";
            hexarray[87] = "57"; hexarray[88] = "58"; hexarray[89] = "59";
            hexarray[90] = "5a"; hexarray[91] = "5b"; hexarray[92] = "5c";
            hexarray[93] = "5d"; hexarray[94] = "5e"; hexarray[95] = "6f";
            hexarray[96] = "60"; hexarray[97] = "61"; hexarray[98] = "62";
            hexarray[99] = "63"; hexarray[100] = "64"; hexarray[101] = "65";
            hexarray[102] = "66"; hexarray[103] = "67"; hexarray[104] = "68";
            hexarray[105] = "69"; hexarray[106] = "6a"; hexarray[107] = "6b";
            hexarray[108] = "6c"; hexarray[109] = "6d"; hexarray[110] = "6e";
            hexarray[111] = "6f"; hexarray[112] = "70"; hexarray[113] = "71";
            hexarray[114] = "72"; hexarray[115] = "73"; hexarray[116] = "74";
            hexarray[117] = "75"; hexarray[118] = "76"; hexarray[119] = "77";
            hexarray[120] = "78"; hexarray[121] = "79"; hexarray[122] = "7a";
            hexarray[123] = "7b"; hexarray[124] = "7c"; hexarray[125] = "7d";
            hexarray[126] = "7e"; hexarray[127] = "7f"; hexarray[128] = "80";
            hexarray[129] = "81"; hexarray[130] = "82"; hexarray[131] = "83";
            hexarray[132] = "84"; hexarray[133] = "85"; hexarray[134] = "86";
            hexarray[135] = "87"; hexarray[136] = "88"; hexarray[137] = "89";
            hexarray[138] = "8a"; hexarray[139] = "8b"; hexarray[140] = "8c";
            hexarray[141] = "8d"; hexarray[142] = "8e"; hexarray[143] = "8f";
            hexarray[144] = "90"; hexarray[145] = "91"; hexarray[146] = "92";
            hexarray[147] = "93"; hexarray[148] = "94"; hexarray[149] = "95";
            hexarray[150] = "96"; hexarray[151] = "97"; hexarray[152] = "98";
            hexarray[153] = "99"; hexarray[154] = "9a"; hexarray[155] = "9b";
            hexarray[156] = "9c"; hexarray[157] = "9d"; hexarray[158] = "9e";
            hexarray[159] = "9f"; hexarray[160] = "a0"; hexarray[161] = "a1";
            hexarray[162] = "a2"; hexarray[163] = "a3"; hexarray[164] = "a4";
            hexarray[165] = "a5"; hexarray[166] = "a6"; hexarray[167] = "a7";
            hexarray[168] = "a8"; hexarray[169] = "a9"; hexarray[170] = "aa";
            hexarray[171] = "aB"; hexarray[172] = "ac"; hexarray[173] = "ad";
            hexarray[174] = "ae"; hexarray[175] = "af"; hexarray[176] = "b0";
            hexarray[177] = "b1"; hexarray[178] = "b2"; hexarray[179] = "b3";
            hexarray[180] = "b4"; hexarray[181] = "b5"; hexarray[182] = "b6";
            hexarray[183] = "b7"; hexarray[184] = "b8"; hexarray[185] = "b9";
            hexarray[186] = "ba"; hexarray[187] = "bb"; hexarray[188] = "bc";
            hexarray[189] = "bd"; hexarray[190] = "be"; hexarray[191] = "bf";
            hexarray[192] = "c0"; hexarray[193] = "c1"; hexarray[194] = "c2";
            hexarray[195] = "c3"; hexarray[196] = "c4"; hexarray[197] = "c5";
            hexarray[198] = "c6"; hexarray[199] = "c7"; hexarray[200] = "c8";
            hexarray[201] = "c9"; hexarray[202] = "ca"; hexarray[203] = "cb";
            hexarray[204] = "cc"; hexarray[205] = "cd"; hexarray[206] = "ce";
            hexarray[207] = "cf"; hexarray[208] = "d0"; hexarray[209] = "d1";
            hexarray[210] = "d2"; hexarray[211] = "d3"; hexarray[212] = "d4";
            hexarray[213] = "d5"; hexarray[214] = "d6"; hexarray[215] = "d7";
            hexarray[216] = "d8"; hexarray[217] = "d9"; hexarray[218] = "da";
            hexarray[219] = "db"; hexarray[220] = "dc"; hexarray[221] = "dd";
            hexarray[222] = "de"; hexarray[223] = "df"; hexarray[224] = "e0";
            hexarray[225] = "e1"; hexarray[226] = "e2"; hexarray[227] = "e3";
            hexarray[228] = "e4"; hexarray[229] = "e5"; hexarray[230] = "e6";
            hexarray[231] = "e7"; hexarray[232] = "e8"; hexarray[233] = "e9";
            hexarray[234] = "ea"; hexarray[235] = "eb"; hexarray[236] = "ec";
            hexarray[237] = "ed"; hexarray[238] = "ee"; hexarray[239] = "ef";
            hexarray[240] = "f0"; hexarray[241] = "f1"; hexarray[242] = "f2";
            hexarray[243] = "f3"; hexarray[244] = "f4"; hexarray[245] = "f5";
            hexarray[246] = "f6"; hexarray[247] = "f7"; hexarray[248] = "f8";
            hexarray[249] = "f9"; hexarray[250] = "fa"; hexarray[251] = "fb";
            hexarray[252] = "fc"; hexarray[253] = "fd"; hexarray[254] = "fe";
            hexarray[255] = "ff";
            return "#" + hexarray[red] + hexarray[green] + hexarray[blue];
        }
    });
})(jQuery);   