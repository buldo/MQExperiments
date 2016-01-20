import java.util.Objects;
import java.util.Scanner;

/**
 * Created by BuldyginRA on 15.01.2016.
 */
public class ServerRunner {
    public static void main(String[] args) {
        NanoMsgServer server = new NanoMsgServer();
        server.start();

        Scanner scanner = new Scanner(System.in);
        System.out.println("Введите \"exit\" для выхода ");
        while(true)
        {
            String line = scanner.nextLine();
            if(Objects.equals(line, "exit"))
            {
                server.stop();
                System.out.println("Сервер остановлен");
                break;
            }
        }
    }
}
