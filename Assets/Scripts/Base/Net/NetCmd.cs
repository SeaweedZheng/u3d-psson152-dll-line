//���������͸��ͻ��˵���Ϣ
public enum S2C_CMD
{
    S2C_HeartHeat = 1000,                      //����
    S2C_WinJackpot,                            //��òʽ�
    S2C_Error,                                 //����
}
//�ͻ��˷��͸�����������Ϣ
public enum C2S_CMD
{
    C2S_HeartHeat = 2000,                      //����
    C2S_Login,                                 //��¼
    C2S_JackBet,                               //��ע
    C2S_ReceiveJackpot,                        //��ȡ�ʽ�
}