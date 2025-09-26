import authAxios from "./authAxios";
import { NotificationStatus } from "./enums";

export interface NotificationResponse {
  id: number;
  text: string;
  createdAt: string;
  status: NotificationStatus;
}

export async function getNotifications(
  skip = 0,
  take = 10
): Promise<NotificationResponse[]> {
  const response = await authAxios.get(`notifications`, {
    params: { skip, take },
  });
  return response.data;
}

export async function markNotificationAsRead(
  notificationId: number
): Promise<boolean> {
  try {
    await authAxios.put(`notification/${notificationId}/read`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function deleteNotification(
  notificationId: number
): Promise<boolean> {
  try {
    await authAxios.put(`notification/${notificationId}/delete`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}
